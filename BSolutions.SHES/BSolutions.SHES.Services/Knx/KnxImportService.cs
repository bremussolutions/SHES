using BSolutions.SHES.Data.Repositories.Projects;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Enumerations;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Projects;
using BSolutions.SHES.Shared.Extensions;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.UI.Core;

namespace BSolutions.SHES.Services.Knx
{
    /// <summary>Executes the import of an ETS file (KNX).</summary>
    public class KnxImportService : ServiceBase, IKnxImportService
    {
        #region --- Fields ---

        private KnxImportOptions _options;
        private KnxImportResult _result;
        private Project _project = new();
        private List<ProjectItem> _projectItems;

        private int _schemaVersion;
        private string _schemaNamespace => $"http://knx.org/xml/project/{_schemaVersion}";
        private XElement _projectXml;
        private XElement _topologyXml;
        private XElement _tradesXml;
        private XElement _locationsXml;

        private readonly IProjectRepository _projectRepository;

        /// <summary>The project type mapping dictionary.</summary>
        /// <remarks>Mapping is necessary because the project types in the ETS have different names than in SHES.</remarks>
        private static readonly Dictionary<string, string> projectTypeMapping = new()
        {
            { "Building", "Building" },
            { "", "BuildingPart" },
            { "Floor", "Floor" },
            { "Room", "Room" },
            { "Corridor", "Corridor" },
            { "DistributionBoard", "Cabinet" }
        };

        #endregion

        /// <summary>Initializes a new instance of the <see cref="KnxImportService" /> class.</summary>
        /// <param name="resourceLoader">The resource loader.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="projectRepository">The project repository.</param>
        public KnxImportService(ResourceLoader resourceLoader, ILogger<KnxImportService> logger, IProjectRepository projectRepository)
            : base(resourceLoader, logger) => _projectRepository = projectRepository;

        #region --- IKnxImportService ---

        /// <summary>Checks asynchronously whether the KNX project is protected with a password.</summary>
        /// <param name="path">The path of KNX project.</param>
        /// <returns>
        ///   Returns the result of the check.
        /// </returns>
        public async Task<bool> ProtectionCheckAsync(string path) => await Task.Run(() =>
        {
            using var file = File.OpenRead(path);
            using var zip = new ZipFile(file);
            return zip.Cast<ZipEntry>().Any(entry => Regex.IsMatch(entry.Name, @"P-\w{4}.zip$"));
        });

        /// <summary>Imports the project asynchronous.</summary>
        /// <param name="path">The path.</param>
        /// <param name="options">Options for the project import.</param>
        /// <param name="password">The password.</param>
        /// <returns>Returns a result with project data.</returns>
        public async Task<KnxImportResult> ImportProjectAsync(string path, KnxImportOptions options, string password = "")
        {
            this._options = options;
            this._result = new KnxImportResult();

            // Import project file
            using var file = File.OpenRead(path);
            await this.UnzipAsync(file, password);

            if (this._result.Successful)
            {
                // Add new project
                if (!await this._projectRepository.ExistsAsync(this._project.Name))
                {
                    await this._projectRepository.AddAsync(this._project);
                    this._result.Data.Project = new ObservableProject(this._project);
                }
                else
                {
                    this._result.ErrorMessage = $"A project with name '{this._result.Data.Project.Name}' already exists. Please delete the existing project in SHES.";
                    this._result.Successful = false;
                }
            }

            return this._result;
        }

        #endregion

        /// <summary>Unzips files from an ETS project file and processes them</summary>
        /// <param name="file">The file to unzip.</param>
        /// <param name="password">The zip file password.</param>
        private async Task UnzipAsync(Stream file, string password)
        {
            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Position = 0;

                using var zip = new ZipFile(ms);
                zip.Password = password;

                // ETS Schema Version
                foreach (ZipEntry zipEntry in zip)
                {
                    if (zipEntry.Name == "knx_master.xml" && zipEntry.IsFile)
                    {
                        await this.ReadEtsSchemaVersionAsync(zipEntry, zip);
                    }
                }

                // Products
                foreach (ZipEntry zipEntry in zip)
                {
                    // Product Catalogs
                    if (Regex.IsMatch(zipEntry.Name, @"^M-\w{4}/Hardware.xml") && zipEntry.IsFile)
                    {
                        await this.ReadHardwareAsync(zipEntry, zip);
                    }
                }

                // Project
                foreach (ZipEntry zipEntry in zip)
                {
                    // Password protected project
                    if (Regex.IsMatch(zipEntry.Name, @"P-\w{4}.zip$") && zipEntry.IsFile)
                    {
                        if (this._schemaVersion >= 21)
                        {
                            await this.UnzipAsync(zip.GetInputStream(zipEntry), this.EncryptEtsPassword(password));
                        }
                        else
                        {
                            await this.UnzipAsync(zip.GetInputStream(zipEntry), password);
                        }
                    }

                    // Project XML
                    if ((Regex.IsMatch(zipEntry.Name, @"^project.xml$") || Regex.IsMatch(zipEntry.Name, @"^P-\w{4}/project.xml$")) && zipEntry.IsFile)
                    {
                        await this.ReadProjectAsync(zipEntry, zip);
                    }

                    // Structure XML
                    if ((Regex.IsMatch(zipEntry.Name, @"^0.xml$") || Regex.IsMatch(zipEntry.Name, @"^P-\w{4}/0.xml$")) && zipEntry.IsFile)
                    {
                        await this.ReadStructureFileAsync(zipEntry, zip);
                    }
                }
            }
            catch (ZipException)
            {
                this._result.Successful = false;
                this._result.ErrorMessage = this._resourceLoader.GetString("Main_ProjectList_EtsImport_PasswordError");
            }
            catch (Exception ex)
            {
                this._result.Successful = false;
                this._result.ErrorMessage = ex.Message;
            }
        }

        /// <summary>Reads the XML schema version of the ETS project file asynchronous.</summary>
        /// <param name="zipEntry">The zip entry.</param>
        /// <param name="zip">The zip.</param>
        /// <remarks>The ETS version with which the project was last processed can be derived from the version of the XML schema.</remarks>
        private async Task ReadEtsSchemaVersionAsync(ZipEntry zipEntry, ZipFile zip)
        {
            await Task.Run(() =>
            {
                var stream = zip.GetInputStream(zipEntry);
                var ns = XElement.Load(stream).GetDefaultNamespace().NamespaceName;
                this._schemaVersion = Convert.ToInt32(ns.Split('/').Last());
            });
        }

        /// <summary>Reads out all products that are used in the ETS project asynchronous.</summary>
        /// <param name="zipEntry">The zip entry.</param>
        /// <param name="zip">The zip.</param>
        /// <remarks>The information is located directly in the root directory of the project file in the M-{XXXX} folders</remarks>
        private async Task ReadHardwareAsync(ZipEntry zipEntry, ZipFile zip)
        {
            var stream = zip.GetInputStream(zipEntry);
            StreamReader reader = new(stream);
            XDocument doc = XDocument.Parse(await reader.ReadToEndAsync());

            var element = doc.Descendants(XName.Get("Hardware", _schemaNamespace)).FirstOrDefault();

            foreach (XElement hardware in element.Elements(XName.Get("Hardware", _schemaNamespace)))
            {
                KnxProduct product = new KnxProduct
                {
                    Name = hardware.Attribute(XName.Get("Name")).Value
                };

                var details = hardware.Element(XName.Get("Products", _schemaNamespace))
                    .Element(XName.Get("Product", _schemaNamespace));

                product.Id = details.Attribute(XName.Get("Id")).Value;
                product.OrderNumber = details.Attribute(XName.Get("OrderNumber")).Value;

                this._result.Data.Products.Add(product);
            }
        }

        /// <summary>Reads the project asynchronous.</summary>
        /// <param name="zipEntry">The zip entry.</param>
        /// <param name="zip">The zip.</param>
        /// <remarks>This is the project.xml in the project folder P-{XXXX}.</remarks>
        private async Task ReadProjectAsync(ZipEntry zipEntry, ZipFile zip)
        {
            using var stream = zip.GetInputStream(zipEntry);
            using var reader = new StreamReader(stream);

            XDocument doc = XDocument.Parse(await reader.ReadToEndAsync().ConfigureAwait(false));
            this._projectXml = doc.Element(XName.Get("KNX", _schemaNamespace));

            var projectElement = this._projectXml?.Element(XName.Get("Project", _schemaNamespace));
            var projectInfo = projectElement?.Element(XName.Get("ProjectInformation", _schemaNamespace));

            this._project.Number = projectElement?.Attribute(XName.Get("Id"))?.Value ?? "UNKNOWN";
            this._project.Name = projectInfo?.Attribute(XName.Get("Name"))?.Value ?? "UNKNOWN";
        }

        /// <summary>Reads the structure file asynchronous.</summary>
        /// <param name="zipEntry">The zip entry.</param>
        /// <param name="zip">The zip.</param>
        /// <remarks>This is the 0.xml in the project folder P-{XXXX}.</remarks>
        private async Task ReadStructureFileAsync(ZipEntry zipEntry, ZipFile zip)
        {
            using var stream = zip.GetInputStream(zipEntry);
            using var reader = new StreamReader(stream);

            var xml = XDocument.Parse(await reader.ReadToEndAsync().ConfigureAwait(false));

            // Verwende FirstOrDefault() direkt für eindeutige Elemente
            this._topologyXml = xml.Descendants(XName.Get("Topology", _schemaNamespace)).FirstOrDefault();
            this._locationsXml = xml.Descendants(XName.Get("Locations", _schemaNamespace)).FirstOrDefault();
            this._tradesXml = xml.Descendants(XName.Get("Trades", _schemaNamespace)).FirstOrDefault();

            // Project Items nur initialisieren, wenn Import aktiviert ist
            if (this._options.ImportStructure && this._locationsXml != null)
            {
                var spaces = this._locationsXml.Elements(XName.Get("Space", _schemaNamespace)) ?? Enumerable.Empty<XElement>();
                this._projectItems = this.ReadProjectItems(spaces);

                this._project.Buildings.AddRange(this._projectItems.Cast<Building>());
            }
        }



        /// <summary>Reads all project items from the ETS project structure.</summary>
        /// <param name="spaces">The spaces (containers that can contain devices).</param>
        /// <returns>Returns a hierarchical list of all project items.<br /></returns>
        private List<ProjectItem> ReadProjectItems(IEnumerable<XElement> spaces)
        {
            return spaces.Select(space =>
            {
                if (!projectTypeMapping.TryGetValue(space.Attribute("Type")?.Value, out string mappedType) || string.IsNullOrEmpty(mappedType))
                {
                    return null; // Falls der Typ nicht gemappt werden kann, ignorieren
                }

                Assembly assembly = typeof(EntityBase).Assembly;
                Type type = assembly.GetType($"BSolutions.SHES.Models.Entities.{mappedType}");
                if (type == null)
                {
                    return null; // Falls der Typ nicht existiert, ignorieren
                }

                ProjectItem entity = (ProjectItem)Activator.CreateInstance(type);
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .ToDictionary(p => p.Name); // Dictionary für schnelleren Zugriff

                // Name setzen
                properties["Name"].SetValue(entity, space.Attribute("Name")?.Value ?? "UNKNOWN");

                // Kinder (rekursive Räume & Geräte)
                var projectItems = ReadProjectItems(space.Elements(XName.Get("Space", _schemaNamespace)));
                if (this._options.ImportDevices)
                {
                    projectItems.AddRange(ReadDevices(space.Elements(XName.Get("DeviceInstanceRef", _schemaNamespace))));
                }
                properties["Children"].SetValue(entity, projectItems);

                return entity;
            })
            .Where(e => e != null)
            .ToList();
        }


        /// <summary>Reads all devices within a space (container).</summary>
        /// <param name="deviceInstanceReferences">The device instance references.</param>
        /// <returns>Returns a list of devices.</returns>
        private List<Device> ReadDevices(IEnumerable<XElement> deviceInstanceReferences)
        {
            return deviceInstanceReferences.Select(deviceInstanceReference =>
            {
                var refId = deviceInstanceReference.Attribute("RefId")?.Value;
                var deviceInstance = _topologyXml.Descendants(XName.Get("DeviceInstance", _schemaNamespace))
                                                 .FirstOrDefault(e => e.Attribute("Id")?.Value == refId);
                if (deviceInstance == null)
                    return null;

                var productRefId = deviceInstance.Attribute("ProductRefId")?.Value;
                var product = _result.Data.Products.FirstOrDefault(p => p.Id == productRefId);

                return new Device
                {
                    BusType = BusType.Knx,
                    Name = string.IsNullOrEmpty(deviceInstance.Attribute("Name")?.Value) ? product?.Name : deviceInstance.Attribute("Name")?.Value,
                    Comment = deviceInstance.Attribute("Comment")?.Value,
                    Description = deviceInstance.Attribute("Description")?.Value,
                    KnxTopologyArea = TryParseInt(deviceInstance.Parent?.Parent?.Parent?.Attribute("Address")?.Value),
                    KnxTopologyLine = TryParseInt(deviceInstance.Parent?.Parent?.Attribute("Address")?.Value),
                    KnxTopologyAddress = TryParseInt(deviceInstance.Attribute("Address")?.Value)
                };
            }).Where(device => device != null).ToList();
        }

        /// <summary>Encrypts a password, as this is required from ETS6 to unpack the project file.</summary>
        /// <param name="password">The plain text password.</param>
        /// <returns>Returns the encrypted ETS project password.</returns>
        private string EncryptEtsPassword(string password)
        {
            int iterations = 65536;
            byte[] salt = Encoding.ASCII.GetBytes("21.project.ets.knx.org");

            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(password), salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] bytes = deriveBytes.GetBytes(32);
                return Convert.ToBase64String(bytes);
            }
        }

        private int? TryParseInt(string value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}

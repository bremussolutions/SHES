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

namespace BSolutions.SHES.Services.Knx
{
    public class KnxImportService : ServiceBase, IKnxImportService
    {
        private KnxImportOptions _options;
        private KnxImportResult _result;
        private Project _project = new();
        private List<ProjectItem> _projectItems;

        private readonly IProjectRepository _projectRepository;

        #region --- Constructor ---

        public KnxImportService (ResourceLoader resourceLoader, ILogger<KnxImportService> logger, IProjectRepository projectRepository)
            : base(resourceLoader, logger)
        {
            this._projectRepository = projectRepository;
        }

        #endregion

        #region --- IKnxImportService ---

        /// <summary>Checks asynchronously whether the KNX project is protected with a password.</summary>
        /// <param name="path">The path of KNX project.</param>
        /// <returns>
        ///   Returns the result of the check.
        /// </returns>
        public async Task<bool> ProtectionCheckAsync(string path)
        {
            return await Task.Run(() =>
            {
                using var file = File.OpenRead(path);
                using var zip = new ZipFile(file);
                foreach (ZipEntry zipEntry in zip)
                {
                    if (Regex.IsMatch(zipEntry.Name, @"P-\w{4}.zip$"))
                    {
                        return true;
                    }
                }

                return false;
            });
        }

        /// <summary>Imports the project asynchronous.</summary>
        /// <param name="path">The path.</param>
        /// <param name="options">Options for the project import.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   Returns a result with project data.
        /// </returns>
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

        #region --- Helper ---

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

                foreach (ZipEntry zipEntry in zip)
                {
                    // ETS Schema Version
                    if (zipEntry.Name == "knx_master.xml" && zipEntry.IsFile)
                    {
                        await this.ReadEtsSchemaVersion(zipEntry, zip);
                    }

                    // Password protected project
                    if (Regex.IsMatch(zipEntry.Name, @"P-\w{4}.zip$") && zipEntry.IsFile)
                    {
                        if (this._result.Data.SchemaVersion >= 21)
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
                        await this.ReadProjectFile(zipEntry, zip);
                    }

                    // Structure XML
                    if ((Regex.IsMatch(zipEntry.Name, @"^0.xml$") || Regex.IsMatch(zipEntry.Name, @"^P-\w{4}/0.xml$")) && zipEntry.IsFile)
                    {
                        await this.ReadStructureFile(zipEntry, zip);
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

        private async Task ReadEtsSchemaVersion(ZipEntry zipEntry, ZipFile zip)
        {
            await Task.Run(() =>
            {
                var stream = zip.GetInputStream(zipEntry);
                var ns = XElement.Load(stream).GetDefaultNamespace().NamespaceName;
                this._result.Data.SchemaVersion = Convert.ToInt32(ns.Split('/').Last());
            });
        }

        private async Task ReadProjectFile(ZipEntry zipEntry, ZipFile zip)
        {
            var stream = zip.GetInputStream(zipEntry);
            StreamReader reader = new(stream);

            this._result.Data.ProjectXml = XDocument.Parse(await reader.ReadToEndAsync());

            this._project.Number = this._result.Data.ProjectXml
                .Element(XName.Get("KNX", this._result.Data.SchemaNamespace))
                .Element(XName.Get("Project", this._result.Data.SchemaNamespace))
                .Attribute(XName.Get("Id")).Value;

            this._project.Name = this._result.Data.ProjectXml
                .Element(XName.Get("KNX", this._result.Data.SchemaNamespace))
                .Element(XName.Get("Project", this._result.Data.SchemaNamespace))
                .Element(XName.Get("ProjectInformation", this._result.Data.SchemaNamespace))
                .Attribute(XName.Get("Name")).Value;
        }

        private async Task ReadStructureFile(ZipEntry zipEntry, ZipFile zip)
        {
            var stream = zip.GetInputStream(zipEntry);
            StreamReader reader = new(stream);

            var xml = XDocument.Parse(await reader.ReadToEndAsync());

            // Topology
            this._result.Data.TopologyXml = xml.Descendants(XName.Get("Topology", this._result.Data.SchemaNamespace)).FirstOrDefault();

            // Locations
            this._result.Data.LocationsXml = xml.Descendants(XName.Get("Locations", this._result.Data.SchemaNamespace)).FirstOrDefault();

            // Trades
            this._result.Data.TradesXml = xml.Descendants(XName.Get("Trades", this._result.Data.SchemaNamespace)).FirstOrDefault();

            // Project Items
            if (this._options.ImportStructure)
            {
                var spaces = this._result.Data.LocationsXml.Elements(XName.Get("Space", this._result.Data.SchemaNamespace));
                this._projectItems = this.LoadProjectItems(spaces);

                this._project.Buildings.AddRange(this._projectItems.Cast<Building>());
            }
        }

        private List<ProjectItem> LoadProjectItems(IEnumerable<XElement> spaces)
        {
            List<ProjectItem> result = new();

            foreach(XElement space in spaces)
            {
                Assembly assembly = typeof(EntityBase).Assembly;
                Type type = assembly.GetType($"BSolutions.SHES.Models.Entities.{ projectTypeMapping[space.Attribute("Type").Value] }");
                ProjectItem entity = (ProjectItem)Activator.CreateInstance(type);
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Locations
                var projectItems = LoadProjectItems(space.Elements(XName.Get("Space", this._result.Data.SchemaNamespace)));

                // Devices
                if (this._options.ImportDevices)
                {
                    projectItems.AddRange(this.LoadDevices(space.Elements(XName.Get("DeviceInstanceRef", this._result.Data.SchemaNamespace))));
                }

                propertyInfos.First(p => p.Name == "Name").SetValue(entity, space.Attribute("Name").Value);
                propertyInfos.First(p => p.Name == "Children").SetValue(entity, projectItems);

                result.Add(entity);
            }

            return result;
        }

        private List<Device> LoadDevices(IEnumerable<XElement> deviceInstanceReferences)
        {
            List<Device> devices = new();

            foreach (XElement deviceInstanceReference in deviceInstanceReferences)
            {
                Device device = new Device() { BusType = BusType.Knx };

                string refId = deviceInstanceReference.Attribute("RefId").Value;
                var deviceInstances = this._result.Data.TopologyXml.Descendants(XName.Get("DeviceInstance", this._result.Data.SchemaNamespace));
                var deviceInstance = deviceInstances.Where(e => e.Attribute("Id").Value == refId).First();
                
                // Basic Infomation
                device.Name = deviceInstance.Attribute("Name").Value;
                device.Comment = deviceInstance.Attribute("Comment")?.Value;
                device.Description = deviceInstance.Attribute("Description")?.Value;

                // Topology
                string area = deviceInstance.Parent.Parent.Parent.Attribute("Address")?.Value;
                string line = deviceInstance.Parent.Parent.Attribute("Address")?.Value;
                string address = deviceInstance.Attribute("Address")?.Value;

                if (!string.IsNullOrEmpty(area) && !string.IsNullOrEmpty(line) && !string.IsNullOrEmpty(address))
                {
                    device.KnxTopologyArea = Convert.ToInt32(area);
                    device.KnxTopologyLine = Convert.ToInt32(line);
                    device.KnxTopologyAddress = Convert.ToInt32(address);
                }

                devices.Add(device);
            }

            return devices;
        }

        /// <summary>Encrypts a password, as this is required from ETS6 to unpack the project file.</summary>
        /// <param name="password">The plain text password.</param>
        /// <returns>
        ///   Returns the encrypted ETS project password.
        /// </returns>
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
    }
}

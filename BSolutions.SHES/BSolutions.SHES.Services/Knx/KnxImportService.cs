using BSolutions.SHES.Data.Repositories.Projects;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using BSolutions.SHES.Services.Projects;
using BSolutions.SHES.Shared.Extensions;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BSolutions.SHES.Services.Knx
{
    public class KnxImportService : ServiceBase, IKnxImportService
    {
        private KnxImportOptions _options;
        private KnxImportResult _result;
        private Project _project;
        private List<ProjectItem> _projectItems;

        private readonly IProjectRepository _projectRepository;

        #region --- Constructor ---

        public KnxImportService(ILogger<KnxImportService> logger, IProjectRepository projectRepository)
            : base(logger)
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
                using (var file = File.OpenRead(path))
                using (var zip = new ZipFile(file))
                {
                    foreach (ZipEntry zipEntry in zip)
                    {
                        if (Regex.IsMatch(zipEntry.Name, @"P-\w{4}.zip$"))
                        {
                            return true;
                        }
                    }

                    return false;
                }
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

            try
            {
                // Import project file
                using var file = File.OpenRead(path);
                await this.UnzipAsync(file, password);

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
            catch(Exception ex)
            {
                this._logger.LogError(ex, "Import not successfully!");
                this._result.ErrorMessage = ex.Message;
                this._result.Successful = false;
            }

            return this._result;
        }

        #endregion

        private async Task UnzipAsync(Stream file, string password)
        {
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                ms.Position = 0;

                using (var zip = new ZipFile(ms))
                {
                    zip.Password = password;

                    foreach (ZipEntry zipEntry in zip)
                    {
                        // Password protected project
                        if (Regex.IsMatch(zipEntry.Name, @"P-\w{4}.zip$") && zipEntry.IsFile)
                        {
                            await this.UnzipAsync(zip.GetInputStream(zipEntry), password);
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
            }
        }

        private async Task ReadProjectFile(ZipEntry zipEntry, ZipFile zip)
        {
            var stream = zip.GetInputStream(zipEntry);
            StreamReader reader = new StreamReader(stream);

            this._result.Data.ProjectXml = XDocument.Parse(await reader.ReadToEndAsync());

            this._project = new Project();

            this._project.Number = this._result.Data.ProjectXml
                .Element(XName.Get("KNX", "http://knx.org/xml/project/20"))
                .Element(XName.Get("Project", "http://knx.org/xml/project/20"))
                .Attribute(XName.Get("Id")).Value;

            this._project.Name = this._result.Data.ProjectXml
                .Element(XName.Get("KNX", "http://knx.org/xml/project/20"))
                .Element(XName.Get("Project", "http://knx.org/xml/project/20"))
                .Element(XName.Get("ProjectInformation", "http://knx.org/xml/project/20"))
                .Attribute(XName.Get("Name")).Value;
        }

        private async Task ReadStructureFile(ZipEntry zipEntry, ZipFile zip)
        {
            var stream = zip.GetInputStream(zipEntry);
            StreamReader reader = new StreamReader(stream);

            var xml = XDocument.Parse(await reader.ReadToEndAsync());

            // Topology
            this._result.Data.TopologyXml = xml.Descendants(XName.Get("Topology", "http://knx.org/xml/project/20")).FirstOrDefault();

            // Locations
            this._result.Data.LocationsXml = xml.Descendants(XName.Get("Locations", "http://knx.org/xml/project/20")).FirstOrDefault();

            // Trades
            this._result.Data.TradesXml = xml.Descendants(XName.Get("Trades", "http://knx.org/xml/project/20")).FirstOrDefault();

            // Project Items
            if (this._options.ImportStructure)
            {
                var spaces = this._result.Data.LocationsXml.Elements(XName.Get("Space", "http://knx.org/xml/project/20"));
                this._projectItems = this.LoadProjectItems(spaces);

                this._project.Buildings.AddRange(this._projectItems.Cast<Building>());
            }
        }

        private List<ProjectItem> LoadProjectItems(IEnumerable<XElement> spaces)
        {
            List<ProjectItem> result = new List<ProjectItem>();

            foreach(XElement space in spaces)
            {
                Assembly assembly = typeof(EntityBase).Assembly;
                Type type = assembly.GetType($"BSolutions.SHES.Models.Entities.{ projectTypeMapping[space.Attribute("Type").Value] }");
                ProjectItem entity = (ProjectItem)Activator.CreateInstance(type);
                PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                // Locations
                var projectItems = LoadProjectItems(space.Elements(XName.Get("Space", "http://knx.org/xml/project/20")));

                // Devices
                if (this._options.ImportDevices)
                {
                    projectItems.AddRange(this.LoadDevices(space.Elements(XName.Get("DeviceInstanceRef", "http://knx.org/xml/project/20"))));
                }

                propertyInfos.First(p => p.Name == "Name").SetValue(entity, space.Attribute("Name").Value);
                propertyInfos.First(p => p.Name == "Children").SetValue(entity, projectItems);

                result.Add(entity);
            }

            return result;
        }

        private List<Device> LoadDevices(IEnumerable<XElement> deviceInstanceReferences)
        {
            List<Device> devices = new List<Device>();

            try
            {
                foreach (XElement deviceInstanceReference in deviceInstanceReferences)
                {
                    string refId = deviceInstanceReference.Attribute("RefId").Value;
                    var deviceInstances = this._result.Data.TopologyXml.Descendants(XName.Get("DeviceInstance", "http://knx.org/xml/project/20"));
                    var deviceInstance = deviceInstances.Where(e => e.Attribute("Id").Value == refId).First();
                    string deviceName = deviceInstance.Attribute("Name").Value;
                    string deviceComment = deviceInstance.Attribute("Comment")?.Value;
                    string deviceDescription = deviceInstance.Attribute("Description")?.Value;

                    devices.Add(new Device { Name = deviceName, Comment = deviceComment, Description = deviceDescription });
                }
            }
            catch (Exception ex) { }

            return devices;
        }

        private static Dictionary<string, string> projectTypeMapping = new Dictionary<string, string>()
        {
            { "Building", "Building" },
            { "", "BuildingPart" },
            { "Floor", "Floor" },
            { "Room", "Room" },
            { "Corridor", "Corridor" },
            { "DistributionBoard", "Cabinet" }
        };
    }
}

using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace BSolutions.SHES.Services.Knx
{
    public class KnxImportData
    {
        public int SchemaVersion { get; set; }

        public string SchemaNamespace
        {
            get
            {
                return $"http://knx.org/xml/project/{this.SchemaVersion}";
            }
        }

        public XDocument ProjectXml { get; set; }

        public XElement TopologyXml { get; set; }

        public XElement TradesXml { get; set; }

        public XElement LocationsXml { get; set; }

        public ObservableProject Project { get; set; }

        public ObservableCollection<ObservableProjectItem> ProjectItems { get; set; }
    }
}

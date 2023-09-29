using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Models.Observables;
using System.Collections.Generic;
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

        public List<KnxProduct> Products { get; set; } = new List<KnxProduct>();
    }

    public class KnxProduct
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string OrderNumber { get; set; }

        public bool IsRailMounted { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

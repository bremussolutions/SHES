using BSolutions.SHES.Models.Observables;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BSolutions.SHES.Services.Knx
{
    public class KnxImportData
    {
        public ObservableProject Project { get; set; }

        public List<KnxProduct> Products { get; set; } = new List<KnxProduct>();
    }

    public class KnxProduct
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string OrderNumber { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

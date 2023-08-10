using BSolutions.SHES.Models.Attributes;
using BSolutions.SHES.Models.Enumerations;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Gerät", "\U000F0C9D")]
    public class Device : ProjectItem
    {
        public DeviceType Type { get; set; }

        public BusType BusType { get; set; }

        public int? KnxTopologyArea { get; set; }

        public int? KnxTopologyLine { get; set; }

        public int? KnxTopologyAddress { get; set; }
    }
}

using BSolutions.SHES.Models.Attributes;
using BSolutions.SHES.Models.Enumerations;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Schaltaktor", "\U000F0C9D")]
    public class SwitchingActuator : Device
    {
        public int Channels { get; set; }

        public ChannelNames ChannelNames { get; set; }
    }
}

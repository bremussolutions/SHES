using BSolutions.SHES.Models.Attributes;

namespace BSolutions.SHES.Models.Enumerations
{
    public enum BusType
    {
        [BusTypeInfo("\U0000E802")]
        Knx,

        [BusTypeInfo("\U000F1A6B")]
        Mqtt,

        [BusTypeInfo("\U000F1A2F")]
        Shelly,

        [BusTypeInfo("\U000F0AD4")]
        Squeezebox
    }
}

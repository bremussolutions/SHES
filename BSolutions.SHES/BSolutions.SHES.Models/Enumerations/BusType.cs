using BSolutions.SHES.Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Enumerations
{
    public enum BusType
    {
        [Display(Name = "Unbekannt")]
        [BusTypeInfo("\uF29C")]
        Unknown,

        [Display(Name = "KNX")]
        [BusTypeInfo("\uE802")]
        Knx,

        [Display(Name = "MQTT")]
        [BusTypeInfo("\uE803")]
        Mqtt,

        [Display(Name = "Shelly")]
        [BusTypeInfo("\uE800")]
        Shelly,

        [Display(Name = "Squeezebox")]
        [BusTypeInfo("\uE805")]
        Squeezebox,

        [Display(Name = "Homematic")]
        [BusTypeInfo("\uE801")]
        Homematic,

        [Display(Name = "1-wire")]
        [BusTypeInfo("\uE80B")]
        OneWire,

        [Display(Name = "Z-Wave")]
        [BusTypeInfo("\uE806")]
        ZWave,

        [Display(Name = "DALI")]
        [BusTypeInfo("\uE80A")]
        Dali,

        [Display(Name = "Ethernet")]
        [BusTypeInfo("\uE804")]
        Ethernet
    }
}

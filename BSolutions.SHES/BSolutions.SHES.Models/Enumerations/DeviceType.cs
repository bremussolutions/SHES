using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Enumerations
{
    public enum DeviceType
    {
        [Display(Name = "Unbekannt")]
        Unknown = 0,

        [Display(Name = "Schaltaktor")]
        SwitchingActuator = 1,

        [Display(Name = "Dimmaktor")]
        DimmingActuator = 2,

        [Display(Name = "Rollladenaktor")]
        ShutterActuator = 3,

        [Display(Name = "Heizungsaktor")]
        HeatingActuator = 4,

        [Display(Name = "Analogaktor")]
        AnalogActuator = 5,

        [Display(Name = "Binäreingang")]
        BinaryInput = 6,

        [Display(Name = "Sonstiger Aktor")]
        OtherActor = 7,

        [Display(Name = "Schalter / Taster")]
        Switch = 8,

        [Display(Name = "Bewegungs- / Präsenzmelder")]
        MotionDetector = 9,

        [Display(Name = "Sonstiger Sensor")]
        OtherSensor = 10,

        [Display(Name = "Visualisierung")]
        Visualization = 11,
    }
}

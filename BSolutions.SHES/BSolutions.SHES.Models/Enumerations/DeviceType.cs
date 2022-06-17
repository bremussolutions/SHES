using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Enumerations
{
    public enum DeviceType
    {
        [Display(Name = "Unbekannt")]
        Unknown = 0,

        [Display(Name = "Analogaktor")]
        AnalogActuator = 1,

        [Display(Name = "Binäreingang")]
        BinaryInput = 2,

        [Display(Name = "Dimmaktor")]
        DimmingActuator = 3,

        [Display(Name = "Heizungsaktor")]
        HeatingActuator = 4,

        [Display(Name = "Bewegungs- / Präsenzmelder")]
        MotionDetector = 5,

        [Display(Name = "Rollladenaktor")]
        ShutterActuator = 6,

        [Display(Name = "Schalter / Taster")]
        Switch = 7,

        [Display(Name = "Schaltaktor")]
        SwitchingActuator = 8,

        [Display(Name = "Visualisierung")]
        Visualization = 9,

        [Display(Name = "Sensor")]
        Sensor = 10
    }
}

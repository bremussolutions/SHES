using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BSolutions.SHES.Models.Enumerations
{
    public enum DeviceType
    {
        [Description("Unbekannt")]
        Unknown = 0,

        [Description("Analogaktor")]
        AnalogActuator = 1,

        [Description("Analogaktor")]
        BinaryInput = 2,

        [Description("Analogaktor")]
        DimmingActuator = 3,

        [Description("Analogaktor")]
        HeatingActuator = 4,

        [Description("Analogaktor")]
        MotionDetector = 5,

        [Description("Analogaktor")]
        ShutterActuator = 6,

        [Description("Analogaktor")]
        Switch = 7,

        [Description("Analogaktor")]
        SwitchingActuator = 8,

        [Description("Analogaktor")]
        Visualization = 9
    }
}

using BSolutions.SHES.Models.Attributes;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Raum", "Door")]
    [RestrictChildren(typeof(Cabinet), typeof(Device))]
    public class Room : ProjectItem
    {
    }
}

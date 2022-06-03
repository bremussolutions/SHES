using BSolutions.SHES.Models.Attributes;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Raum", "\U000F081A")]
    [RestrictChildren(typeof(Cabinet), typeof(Device))]
    public class Room : ProjectItem
    {
    }
}

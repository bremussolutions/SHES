using BSolutions.SHES.Models.Attributes;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Treppe", "Stairs")]
    [RestrictChildren(typeof(Cabinet), typeof(Device))]
    public class Stair : ProjectItem
    {
    }
}

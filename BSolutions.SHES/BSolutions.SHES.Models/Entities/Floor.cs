using BSolutions.SHES.Models.Attributes;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Etage", "FloorPlan")]
    [RestrictChildren(typeof(Corridor), typeof(Room))]
    public class Floor : ProjectItem
    {
    }
}

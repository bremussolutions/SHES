using BSolutions.SHES.Models.Attributes;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Gebäudeteil", "\U000F0991")]
    [RestrictChildren(typeof(BuildingPart), typeof(Floor), typeof(Corridor), typeof(Stair), typeof(Room))]
    public class BuildingPart : ProjectItem
    {
    }
}

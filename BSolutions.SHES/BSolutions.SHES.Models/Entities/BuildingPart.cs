using BSolutions.SHES.Models.Attributes;

namespace BSolutions.SHES.Models.Entities
{
    [ProjectItemInfo("Gebäudeteil", "OfficeBuilding")]
    [RestrictChildren(typeof(BuildingPart), typeof(Floor), typeof(Corridor), typeof(Stair), typeof(Room))]
    public class BuildingPart : ProjectItem
    {
    }
}

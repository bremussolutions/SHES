namespace BSolutions.SHES.Models
{
    public sealed class ProjectItemTypeInfo
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string FullName { get; set; }

        public override string ToString()
        {
            return $"{FullName}: {DisplayName}";
        }
    }
}

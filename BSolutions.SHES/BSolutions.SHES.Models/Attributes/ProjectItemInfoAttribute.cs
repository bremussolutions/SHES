using System;

namespace BSolutions.SHES.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ProjectItemInfoAttribute : Attribute
    {
        public string DisplayName { get; set; }

        public string Icon { get; set; }

        public ProjectItemInfoAttribute(string displayName, string icon)
        {
            this.DisplayName = displayName;
            this.Icon = icon;
        }
    }
}

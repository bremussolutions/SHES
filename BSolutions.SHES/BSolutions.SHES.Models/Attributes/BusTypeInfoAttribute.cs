using System;

namespace BSolutions.SHES.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class BusTypeInfoAttribute : Attribute
    {
        public string Icon { get; set; }

        public BusTypeInfoAttribute(string icon)
        {
            Icon = icon;
        }
    }
}

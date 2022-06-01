using System;
using System.Collections.Generic;
using System.Linq;

namespace BSolutions.SHES.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class RestrictChildrenAttribute : Attribute
    {
        public List<Type> RestrictedChildrenTypes { get; set; }

        public RestrictChildrenAttribute(params Type[] restrictedChildren)
        {
            this.RestrictedChildrenTypes = restrictedChildren.ToList();
        }
    }
}

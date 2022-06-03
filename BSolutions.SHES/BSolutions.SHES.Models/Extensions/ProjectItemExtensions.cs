using BSolutions.SHES.Models.Attributes;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSolutions.SHES.Models.Extensions
{
    public static class ProjectItemExtensions
    {
        /// <summary>Gets the restrict children types of a project item.</summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>
        ///   Returns the restricted children types of a project item.
        /// </returns>
        public static List<Type> GetRestrictChildrenTypes(this ProjectItem projectItem)
        {
            return projectItem.GetType()
                .GetAttributeValue((RestrictChildrenAttribute attr) => attr.RestrictedChildrenTypes);
        }

        public static List<ProjectItemTypeInfo> GetRestrictChildrenInfos(this ProjectItem projectItem)
        {
            List<ProjectItemTypeInfo> result = new List<ProjectItemTypeInfo>();
            List<Type> restrictChildrenTypes = projectItem.GetRestrictChildrenTypes();

            if (restrictChildrenTypes != null)
            {
                foreach (var type in restrictChildrenTypes)
                {
                    ProjectItemInfoAttribute attribute = type.GetCustomAttributes(typeof(ProjectItemInfoAttribute), true).FirstOrDefault() as ProjectItemInfoAttribute;

                    if (attribute != null)
                    {
                        result.Add(new ProjectItemTypeInfo
                        {
                            Name = type.Name,
                            DisplayName = attribute.DisplayName,
                            FullName = type.FullName,
                            Icon = attribute.Icon
                        });
                    }
                }
            }

            return result;
        }
    }
}

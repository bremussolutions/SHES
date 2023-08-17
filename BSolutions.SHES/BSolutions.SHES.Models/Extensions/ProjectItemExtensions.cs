using BSolutions.SHES.Models.Attributes;
using BSolutions.SHES.Models.Entities;
using BSolutions.SHES.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSolutions.SHES.Models.Extensions
{
    /// <summary>
    /// Provides enhancements in handling project items.
    /// </summary>
    public static class ProjectItemExtensions
    {
        /// <summary>Gets the restrict children types of a project item.</summary>
        /// <param name="projectItem">The project item.</param>
        /// <returns>Returns the restricted children types of a project item.</returns>
        public static List<Type> GetRestrictChildrenTypes(this ProjectItem projectItem)
        {
            return projectItem.GetType()
                .GetAttributeValue((RestrictChildrenAttribute attr) => attr.RestrictedChildrenTypes);
        }

        /// <summary>
        /// Reads out the restricted project items that are valid as child for the current item.
        /// </summary>
        /// <param name="projectItem">The parent project item.</param>
        /// <returns>Returns a list of allowed child item types.</returns>
        public static List<ProjectItemTypeInfo> GetRestrictChildrenInfos(this ProjectItem projectItem)
        {
            List<ProjectItemTypeInfo> result = new();
            List<Type> restrictChildrenTypes = projectItem.GetRestrictChildrenTypes();

            if (restrictChildrenTypes != null)
            {
                foreach (var type in restrictChildrenTypes)
                {
                    if (type.GetCustomAttributes(typeof(ProjectItemInfoAttribute), true).FirstOrDefault() is ProjectItemInfoAttribute attribute)
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

        /// <summary>
        /// This method reads a hierarchical structure and transforms it into a flat list.
        /// </summary>
        /// <typeparam name="T">The content type of the list.</typeparam>
        /// <param name="items">The items of the list from the first hierarchy level.</param>
        /// <param name="childSelector">The child selector.</param>
        /// <param name="filterType">The type which should be considered exclusively.</param>
        /// <returns>Returns a flat list of a hierarchical order.</returns>
        public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector, Type filterType = null)
        {
            var stack = new Stack<T>(items);
            while (stack.Any())
            {
                var next = stack.Pop();
                if (filterType != null)
                {
                    if(next.GetType() == filterType)
                    {
                        yield return next;
                    }
                }
                else
                {
                    yield return next;
                }

                foreach (var child in childSelector(next))
                {
                    stack.Push(child);
                }
            }
        }
    }
}

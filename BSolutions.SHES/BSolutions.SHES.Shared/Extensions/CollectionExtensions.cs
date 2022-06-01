using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BSolutions.SHES.Shared.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>Adds a range of items to an observable collection.</summary>
        /// <typeparam name="T">Type of observable collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }

        /// <summary>Adds a range of items to an observable collection.</summary>
        /// <typeparam name="T">Type of observable collection.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            items.ToList().ForEach(collection.Add);
        }
    }
}

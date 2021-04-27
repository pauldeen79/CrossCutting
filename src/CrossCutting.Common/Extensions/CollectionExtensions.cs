using System.Collections.Generic;
using System.Linq;

namespace CrossCutting.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> instance, IEnumerable<T> itemsToAdd)
        {
            foreach (var itemToAdd in itemsToAdd ?? Enumerable.Empty<T>())
            {
                instance.Add(itemToAdd);
            }

            return instance;
        }
    }
}

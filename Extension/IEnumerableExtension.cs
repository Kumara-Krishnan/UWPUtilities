using System.Collections.Generic;
using System.Linq;

namespace UWPUtilities.Extension
{
    public static class IEnumerableExtension
    {
        public static bool IsNonEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection is null || !collection.Any();
        }
    }
}

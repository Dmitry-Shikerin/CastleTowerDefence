using System.Collections.Generic;
using System.Linq;

namespace Sources.Utils.Extensions
{
    public static class ListExtension
    {
        public static (IEnumerable<T> added, IEnumerable<T> removed) Diff<T>(
            this IEnumerable<T> sourceCollection, IEnumerable<T> changedCollection)
        {
            var removed = sourceCollection.Except(changedCollection);
            var added = changedCollection.Except(sourceCollection);

            return (added, removed);
        }
    }
}
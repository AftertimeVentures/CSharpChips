using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sky7.CSharpChips.Collections {
    /// <summary>
    /// CollectionExtensions static class implements a number of extensions methods to support
    /// advanced scenarios for generic collections.
    /// </summary>
    public static class CollectionExtensions {
        /// <summary>
        /// AddAndReturn extension method adds the supplied item int othe collection and the 
        /// returns that item. AddAndReturn is useful to allow simplier return statements like
        ///     return collection.AddAndReturn(new Item(argument));
        /// instead of the old bulky three-statement menace
        ///     Item item = new Item(argument);
        ///     collection.Add(item);
        ///     return item;
        /// </summary>
        /// <typeparam name="T">Type of collection elements</typeparam>
        /// <param name="collection">The collection to add the item to.</param>
        /// <param name="item">The item to add.</param>
        /// <returns></returns>
        public static T AddAndReturn<T>(this ICollection<T> collection, T item) {
            collection.Add(item);

            return item;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items) {
            IList<T> list = collection as IList<T>;

            if (list != null) {
                list.AddRange(items);
            }  else {
                foreach (T item in items)
                    collection.Add(item);
            }
        }
    }
}

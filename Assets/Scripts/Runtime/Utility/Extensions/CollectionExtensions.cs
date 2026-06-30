using System;
using System.Collections.Generic;

namespace Runtime.Utility.Extensions {
    public static class CollectionExtensions {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach (T item in collection) {
                action(item);
            }
        }

        public static void AddAmount<T>(this List<T> collection, T item, int amount) {
            for (int i = 0; i < amount; i++) {
                collection.Add(item);
            }
        }
    }
}

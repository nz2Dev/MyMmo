using System.Collections.Generic;
using System.Linq;

namespace Player {
    public static class Utils {

        public static string AggregateToString<T>(this IEnumerable<T> enumerable) {
            var source = enumerable.ToArray();
            if (source.Length == 0) {
                return "";
            }
            return source.Select(data => data.ToString()).Aggregate((prev, data) => $"{prev}, {data}");
        }
    
    }
}
using System.Collections.Generic;
using System.Linq;

public static class UtilsCopy {

    public static string AggregateToStringCopy<T>(this IEnumerable<T> enumerable) {
        var source = enumerable.ToArray();
        if (source.Length == 0) {
            return "";
        }
        return source.Select(data => data.ToString()).Aggregate((prev, data) => $"{prev}, {data}");
    }
    
}
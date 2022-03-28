using System.Collections.Generic;

namespace MyMmo.Server.Writers {
    public static class WritersUtils {

        public static IEnumerable<T> ToEnumerable<T>(this T t) {
            yield return t;
        }

    }
}
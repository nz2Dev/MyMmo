using System;

namespace MyMmo.Processing.Utils {
    public static class Assertions {

        public static void AssertIsTrue(bool condition, Func<string> message) {
            if (!condition) {
                throw new Exception(message());
            }
        }

    }
}
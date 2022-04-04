namespace MyMmo.Processing.Utils {
    public static class NumberUtils {

        public static float Map(float value, float rangeStart, float rangeStop, float newRangeStart, float newRangeStop) {
            var range = rangeStop - rangeStart;
            var progress = (value - rangeStart) / range;
            var newRange = newRangeStop - newRangeStart;
            return newRangeStart + newRange * progress;
        }

    }
}
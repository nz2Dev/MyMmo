using MyMmo.Commons.Primitives;

namespace MyMmo.Processing.Utils {
    public static class ConverterExtensions {

        public static Vector2 ToDataVector2(this System.Numerics.Vector2 actualVector2) {
            return new Vector2 {X = actualVector2.X, Y = actualVector2.Y};
        }

        public static System.Numerics.Vector2 ToComputeVector(this Vector2 dataVector) {
            return new System.Numerics.Vector2(dataVector.X, dataVector.Y);
        }
    }
}
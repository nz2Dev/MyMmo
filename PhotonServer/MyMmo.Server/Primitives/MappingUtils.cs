using MyMmo.Commons.Primitives;

namespace MyMmo.Server.Primitives {
    public static class MappingUtils {

        public static Vector2 ToDataVector2(this System.Numerics.Vector2 actualVector2) {
            return new Vector2 {X = actualVector2.X, Y = actualVector2.Y};
        }
    }
}
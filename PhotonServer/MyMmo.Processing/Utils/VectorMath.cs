using System.Numerics;

namespace MyMmo.Processing.Utils {
    public static class VectorMath {

        public static Vector2 VectorProjection(Vector2 a, Vector2 b) {
            var unitB = Vector2.Normalize(b);
            var scalarProjection = Vector2.Dot(a, unitB);
            return unitB * scalarProjection;
        }

    }
}
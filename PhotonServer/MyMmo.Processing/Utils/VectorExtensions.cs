using System.Numerics;

namespace MyMmo.Processing.Utils {
    public static class VectorExtensions {

        public static Vector2 Limit(Vector2 vector2, float magnitude) {
            if (vector2.Length() > magnitude) {
                return Vector2.Normalize(vector2) * magnitude;
            }

            return vector2;
        }

    }
}
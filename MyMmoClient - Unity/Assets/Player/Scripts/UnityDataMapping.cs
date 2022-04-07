using UnityEngine;
using Vector2 = MyMmo.Commons.Primitives.Vector2;

namespace Player.Scripts {
    public static class UnityDataMapping {

        public static Vector3 ToUnityVector3(this Vector2 primitive) {
            return new Vector3(primitive.X, 0, primitive.Y);
        }
    }
}
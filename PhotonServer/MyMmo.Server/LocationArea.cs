using System;
using System.Numerics;

namespace MyMmo.Server {
    public class LocationArea {

        private readonly int id;

        private static Random random = new Random();

        public LocationArea(int id) {
            this.id = id;
        }

        public Vector2 GetRandomPositionWithinBounds() {
            return new Vector2(
                random.Next(-5, 5),
                random.Next(-5, 5)
            );
        }

    }
}
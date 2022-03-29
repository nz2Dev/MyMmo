using System;
using System.Numerics;

namespace MyMmo.Server.Game {
    public class MapRegion {

        private readonly int locationId;

        private static Random random = new Random();

        public MapRegion(int locationId) {
            this.locationId = locationId;
        }

        public Vector2 GetRandomPositionWithinBounds() {
            return new Vector2(
                random.Next(-5, 5),
                random.Next(-5, 5)
            );
        }

    }
}
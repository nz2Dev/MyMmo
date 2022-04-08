using System;
using System.Numerics;

namespace MyMmo.Server.Domain {
    public class MapRegion {

        private readonly int locationId;

        private static Random random = new Random();

        public int locationToTheRight = -1;
        public readonly Vector2 rightExitPoint = new Vector2(5, 0);
        public readonly Vector2 rightEnterPoint = new Vector2(5, 0);

        public int locationToTheLeft = -1;
        public readonly Vector2 leftExitPoint = new Vector2(-5, 0);
        public readonly Vector2 leftEnterPoint = new Vector2(-5, 0);

        public MapRegion(int locationId) {
            this.locationId = locationId;
        }

        public Vector2 GetExitPositionTo(int otherLocationId) {
            if (locationToTheLeft >= 0 && otherLocationId == locationToTheLeft) {
                return leftExitPoint;
            } else if (locationToTheRight >= 0 && otherLocationId == locationToTheRight) {
                return rightExitPoint;
            } else {
                throw new Exception("No exit point to location: " + otherLocationId);
            }
        }

        public Vector2 GetEnterPointFrom(int otherLocationId) {
            if (locationToTheLeft >= 0 && otherLocationId == locationToTheLeft) {
                return leftEnterPoint;
            } else if (locationToTheRight >= 0 && otherLocationId == locationToTheRight) {
                return rightExitPoint;
            } else {
                throw new Exception("No enter point from location: " + otherLocationId);
            }
        }

        public Vector2 GetRandomPositionWithinBounds() {
            return new Vector2(
                random.Next(-5, 5),
                random.Next(-5, 5)
            );
        }

    }
}
using System;
using System.Collections.Generic;

namespace MyMmo.Server {
    public class World /*todo Dispose*/ {

        public const int RootLocationId = 0;
        public const int SecondLocationId = 1;
        public const int ThirdLocationId = 2;

        private readonly Region rootRegion;
        private readonly Region secondRegion;
        private readonly Region thirdRegion;
        private readonly ItemCache itemRegistry = new ItemCache();

        public World() {
            rootRegion = new Region(RootLocationId);
            secondRegion = new Region(SecondLocationId);
            thirdRegion = new Region(ThirdLocationId);
        }

        public Region GetRegion(int locationId) {
            switch (locationId) {
                case RootLocationId: return rootRegion;
                case SecondLocationId: return secondRegion;
                case ThirdLocationId: return thirdRegion;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public HashSet<Region> GetSurroundedRegionsIncluded(int locationId) {
            switch (locationId) {
                case RootLocationId: return new HashSet<Region> {rootRegion, secondRegion};
                case SecondLocationId: return new HashSet<Region> {rootRegion, secondRegion, thirdRegion};
                case ThirdLocationId: return new HashSet<Region> {secondRegion, thirdRegion};
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public static World CreateDefaultWorld() {
            return new World();
        }

    }
}
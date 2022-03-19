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

        private readonly Location rootLocation;
        private readonly Location secondLocation;
        private readonly Location thirdLocation;
        
        private readonly ItemCache itemRegistry = new ItemCache();

        public World() {
            rootRegion = new Region(RootLocationId);
            secondRegion = new Region(SecondLocationId);
            thirdRegion = new Region(ThirdLocationId);
            
            rootLocation = new Location(this, RootLocationId);
            secondLocation = new Location(this, SecondLocationId);
            thirdLocation = new Location(this, ThirdLocationId);
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

        public Location GetLocation(int locationId) {
            switch (locationId) {
                case RootLocationId: return rootLocation;
                case SecondLocationId: return secondLocation;
                case ThirdLocationId: return thirdLocation;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public bool RegisterItem(Item item) {
            return itemRegistry.TryAdd(item);
        }

        public void RemoveItem(Item item) {
            itemRegistry.Remove(item);
        }

        public bool TryGetItem(string itemId, out Item item) {
            return itemRegistry.TryGetItem(itemId, out item);
        }

        public static World CreateDefaultWorld() {
            return new World();
        }

    }
}
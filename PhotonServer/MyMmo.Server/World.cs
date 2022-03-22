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

        private readonly LocationArea rootLocationArea;
        private readonly LocationArea secondLocationArea;
        private readonly LocationArea thirdLocationArea;
        
        private readonly LocationSimulator rootLocationSimulator;
        private readonly LocationSimulator secondLocationSimulator;
        private readonly LocationSimulator thirdLocationSimulator;
        
        private readonly ItemCache itemRegistry = new ItemCache();

        public World() {
            rootRegion = new Region(RootLocationId);
            secondRegion = new Region(SecondLocationId);
            thirdRegion = new Region(ThirdLocationId);
            
            rootLocationArea = new LocationArea(RootLocationId);
            secondLocationArea = new LocationArea(SecondLocationId);
            thirdLocationArea = new LocationArea(ThirdLocationId);
            
            rootLocationSimulator = new LocationSimulator(this, RootLocationId);
            secondLocationSimulator = new LocationSimulator(this, SecondLocationId);
            thirdLocationSimulator = new LocationSimulator(this, ThirdLocationId);
        }

        public Region GetRegion(int locationId) {
            switch (locationId) {
                case RootLocationId: return rootRegion;
                case SecondLocationId: return secondRegion;
                case ThirdLocationId: return thirdRegion;
                default: throw new ArgumentOutOfRangeException($"locationId: {locationId}");
            }
        }

        public HashSet<Region> GetSurroundedRegionsIncluded(int locationId) {
            switch (locationId) {
                case RootLocationId: return new HashSet<Region> {rootRegion, secondRegion};
                case SecondLocationId: return new HashSet<Region> {rootRegion, secondRegion, thirdRegion};
                case ThirdLocationId: return new HashSet<Region> {secondRegion, thirdRegion};
                default: throw new ArgumentOutOfRangeException($"locationId: {locationId}");
            }
        }

        public LocationArea GetLocationArea(int locationId) {
            switch (locationId) {
                case RootLocationId: return rootLocationArea;
                case SecondLocationId: return secondLocationArea;
                case ThirdLocationId: return thirdLocationArea;
                default: throw new ArgumentOutOfRangeException($"locationId: {locationId}");
            }
        }

        public LocationSimulator GetLocationSimulator(int locationId) {
            switch (locationId) {
                case RootLocationId: return rootLocationSimulator;
                case SecondLocationId: return secondLocationSimulator;
                case ThirdLocationId: return thirdLocationSimulator;
                default: throw new ArgumentOutOfRangeException($"locationId: {locationId}");
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

        public Item GetItem(string itemId) {
            return TryGetItem(itemId, out var item) ? item : throw new ItemNotFound(itemId, this);
        }

        public static World CreateDefaultWorld() {
            return new World();
        }

    }
}
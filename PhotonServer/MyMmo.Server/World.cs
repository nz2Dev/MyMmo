using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMmo.Server {
    public class World /*todo Dispose*/ {

        private readonly object syncRoot = new object();

        public const int RootLocationId = 0;
        public const int SecondLocationId = 1;
        public const int ThirdLocationId = 2;

        private readonly LocationArea rootLocationArea;
        private readonly LocationArea secondLocationArea;
        private readonly LocationArea thirdLocationArea;

        private readonly Location rootLocation;
        private readonly Location secondLocation;
        private readonly Location thirdLocation;

        private readonly ItemCache itemRegistry = new ItemCache();

        public World() {
            rootLocationArea = new LocationArea(RootLocationId);
            secondLocationArea = new LocationArea(SecondLocationId);
            thirdLocationArea = new LocationArea(ThirdLocationId);

            rootLocation = new Location(this, RootLocationId);
            secondLocation = new Location(this, SecondLocationId);
            thirdLocation = new Location(this, ThirdLocationId);
        }

        public HashSet<Location> GetSurroundedLocationsIncluded(int locationId) {
            switch (locationId) {
                case RootLocationId: return new HashSet<Location> {rootLocation, secondLocation};
                case SecondLocationId: return new HashSet<Location> {rootLocation, secondLocation, thirdLocation};
                case ThirdLocationId: return new HashSet<Location> {secondLocation, thirdLocation};
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

        public Location GetLocation(int locationId) {
            switch (locationId) {
                case RootLocationId: return rootLocation;
                case SecondLocationId: return secondLocation;
                case ThirdLocationId: return thirdLocation;
                default: throw new ArgumentOutOfRangeException($"locationId: {locationId}");
            }
        }

        public void ExecuteStateScripts(IEnumerable<IScript> scripts) {
            lock (syncRoot) {
                foreach (var script in scripts) {
                    script.ApplyState(this);
                }
            }
        }

        public ICollection<ItemSnapshot> GetItemSnapshotsAtLocation(int locationId) {
            return itemRegistry.GetItemsWithLocationId(locationId).Select(item => item.GenerateItemSnapshot()).ToArray();
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
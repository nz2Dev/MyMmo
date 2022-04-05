using System;
using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Processing;
using MyMmo.Processing.Components;
using MyMmo.Processing.Utils;
using MyMmo.Server.Updates;

namespace MyMmo.Server.Domain {
    public class World /*todo Dispose*/ {

        private readonly object syncRoot = new object();

        public const int RootLocationId = 0;
        public const int SecondLocationId = 1;
        public const int ThirdLocationId = 2;

        private readonly MapRegion rootMapRegion;
        private readonly MapRegion secondMapRegion;
        private readonly MapRegion thirdMapRegion;

        private readonly Location rootLocation;
        private readonly Location secondLocation;
        private readonly Location thirdLocation;

        private readonly ItemCache itemRegistry = new ItemCache();

        public World() {
            rootMapRegion = new MapRegion(RootLocationId);
            secondMapRegion = new MapRegion(SecondLocationId);
            thirdMapRegion = new MapRegion(ThirdLocationId);

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

        public MapRegion GetMapRegion(int locationId) {
            switch (locationId) {
                case RootLocationId: return rootMapRegion;
                case SecondLocationId: return secondMapRegion;
                case ThirdLocationId: return thirdMapRegion;
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

        public ScriptsClipData ExecuteSimulationAt(int locationId, List<BaseServerUpdate> updates) {
            lock (syncRoot) {
                updates.ForEach(update => update.Attach(this));
                
                var entities = itemRegistry.GetItemsWithLocationId(locationId).Select(item => {
                    var transform = new Transform(item.PositionInLocation, item.LocationId, changes => {
                        item.ChangePositionInLocation(changes.ToPosition.ToComputeVector());
                    });
                    return new Entity(item.Id, transform);
                });

                var scene = new Scene(entities, updates.Cast<IUpdate>().ToList());
                var clip = scene.Simulate(stepTime: 0.2f, simulationTime: 2f);    
                return clip;
            }
        }

        public ICollection<ItemSnapshot> GetItemSnapshotsAtLocation(int locationId) {
            return itemRegistry.GetItemsWithLocationId(locationId).Select(item => item.GenerateItemSnapshot()).ToArray();
        }

        public void RegisterItem(Item item) {
            itemRegistry.Add(item);
        }

        public void RemoveItem(Item item) {
            itemRegistry.Remove(item);
        }

        public bool TryGetItem(string itemId, out Item item) {
            return itemRegistry.TryGetItem(itemId, out item);
        }

        public Item GetItem(string itemId) {
            return itemRegistry.GetItem(itemId);
        }

        public bool ContainItem(string itemId) {
            return itemRegistry.Contain(itemId);
        }

        public static World CreateDefaultWorld() {
            return new World();
        }

    }
}
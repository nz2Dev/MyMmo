using System;
using System.Collections.Generic;
using MyMmo.Commons.Scripts;
using MyMmo.Processing;
using MyMmo.Processing.Utils;

namespace MyMmo.Server.Domain {
    public class World /*todo Dispose*/ {

        private static readonly object SyncRoot = new object();

        public const int RootLocationId = 0;
        public const int SecondLocationId = 1;
        public const int ThirdLocationId = 2;
        
        private readonly Location rootLocation;
        private readonly Location secondLocation;
        private readonly Location thirdLocation;

        private readonly ItemCache itemRegistry = new ItemCache();

        public World() {
            rootLocation = new Location(this, RootLocationId, new MapRegion(RootLocationId) {
                locationToTheRight = SecondLocationId
            });
            secondLocation = new Location(this, SecondLocationId, new MapRegion(SecondLocationId) {
                locationToTheLeft = RootLocationId,
                locationToTheRight = ThirdLocationId
            });
            thirdLocation = new Location(this, ThirdLocationId, new MapRegion(ThirdLocationId) {
                locationToTheLeft = SecondLocationId
            });
        }

        public HashSet<Location> GetSurroundedLocationsIncluded(int locationId) {
            switch (locationId) {
                case RootLocationId: return new HashSet<Location> {rootLocation, secondLocation};
                case SecondLocationId: return new HashSet<Location> {rootLocation, secondLocation, thirdLocation};
                case ThirdLocationId: return new HashSet<Location> {secondLocation, thirdLocation};
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

        public void ApplyChanges(int locationId, ScriptsClipData updateData) {
            lock (SyncRoot) {
                foreach (var itemScriptsData in updateData.ItemDataArray) {
                    foreach (var baseScriptData in itemScriptsData.ScriptDataArray) {
                        if (baseScriptData is SpawnItemScriptData spawnItemScriptData) {
                            var item = itemRegistry.GetItem(spawnItemScriptData.EntitySnapshotData.ItemId);
                            item.Spawn(locationId, spawnItemScriptData.EntitySnapshotData);
                            break;
                        }

                        if (baseScriptData is DestroyItemScriptData destroyItemScriptData) {
                            var item = itemRegistry.GetItem(destroyItemScriptData.ItemId);
                            itemRegistry.Remove(item);
                            item.Destroy();
                            item.Dispose();
                            break;
                        }

                        if (baseScriptData is ExitItemScriptData exitItemScriptData) {
                            var item = itemRegistry.GetItem(exitItemScriptData.ItemId);
                            item.DetachFromLocation();
                            break;
                        }

                        if (baseScriptData is EnterItemScriptData enterItemScriptData) {
                            var item = itemRegistry.GetItem(enterItemScriptData.EntitySnapshotData.ItemId);
                            item.AttachToLocation(locationId, enterItemScriptData.EntitySnapshotData);
                            break;
                        }

                        if (baseScriptData is ChangePositionScriptData changePositionScriptData) {
                            var item = itemRegistry.GetItem(changePositionScriptData.ItemId);
                            item.ChangePositionInLocation(changePositionScriptData.ToPosition.ToComputeVector());
                            break;
                        }
                        
                        throw new Exception($"Can't recognise scriptData {baseScriptData} or break; is missing");
                    }
                }
            }
        }

        public void RegisterItem(Item item) {
            itemRegistry.Add(item);
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
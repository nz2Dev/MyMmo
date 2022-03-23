using System.Numerics;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;

namespace MyMmo.Server.Scripts {
    public class SpawnItemScript : IScript {

        private string itemId;
        private int locationId;
        private Vector2 position;

        public SpawnItemScript(string itemId, int locationId, Vector2 position) {
            this.itemId = itemId;
            this.locationId = locationId;
            this.position = position;
        }

        public BaseScriptData ToScriptData() {
            return new SpawnItemScriptData {
                ItemSnapshotData = new ItemSnapshotData {
                    ItemId = itemId,
                    LocationId = locationId,
                    PositionInLocation = position.ToDataVector2()
                }
            };
        }

        public void ApplyState(World world) {
            var itemToSpawn = world.GetItem(itemId);
            itemToSpawn.Spawn(locationId, position);
        }
    }
}
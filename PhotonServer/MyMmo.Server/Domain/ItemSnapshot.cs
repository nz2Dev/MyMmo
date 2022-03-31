using System.Numerics;
using MyMmo.Commons.Snapshots;
using MyMmo.Processing.Utils;

namespace MyMmo.Server.Domain {
    public class ItemSnapshot {

        public readonly string itemId;
        public readonly int locationId;
        public readonly Vector2 positionInLocation;

        public ItemSnapshot(string itemId, int locationId, Vector2 positionInLocation) {
            this.itemId = itemId;
            this.locationId = locationId;
            this.positionInLocation = positionInLocation;
        }

        public ItemSnapshotData ToData() {
            return new ItemSnapshotData {
                ItemId = itemId,
                LocationId = locationId,
                PositionInLocation = positionInLocation.ToDataVector2()
            };
        }
    }
}
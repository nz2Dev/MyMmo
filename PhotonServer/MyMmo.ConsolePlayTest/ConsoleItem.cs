using MyMmo.Commons.Primitives;
using MyMmo.Commons.Snapshots;

namespace MyMmo.ConsolePlayTest {
    public class ConsoleItem {

        public string ItemId { get; }
        public int LocationId { get; set; }
        public Vector2 PositionInLocation { get; set; }
        
        public ConsoleItem(EntitySnapshotData entitySnapshotData) {
            ItemId = entitySnapshotData.ItemId;
            LocationId = entitySnapshotData.LocationId;
            PositionInLocation = entitySnapshotData.PositionInLocation;
        }
    }
}
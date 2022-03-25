using MyMmo.Commons.Primitives;
using MyMmo.Commons.Snapshots;

namespace MyMmo.ConsolePlayTest {
    public class ConsoleItem {

        public string ItemId { get; }
        public int LocationId { get; set; }
        public Vector2 PositionInLocation { get; set; }
        
        public ConsoleItem(ItemSnapshotData itemSnapshotData) {
            ItemId = itemSnapshotData.ItemId;
            LocationId = itemSnapshotData.LocationId;
            PositionInLocation = itemSnapshotData.PositionInLocation;
        }
    }
}
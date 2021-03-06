using MyMmo.Commons.Primitives;
using MyMmo.Commons.Snapshots;

namespace MyMmo.ConsolePlayTest {
    public class ConsoleItem {

        public string ItemId { get; }
        public int LocationId { get; set; }
        public Vector2 PositionInLocation { get; set; }
        
        public ConsoleItem(int locationId, EntitySnapshotData entitySnapshotData) {
            LocationId = locationId;
            ItemId = entitySnapshotData.ItemId;
            PositionInLocation = entitySnapshotData.PositionInLocation;
        }
    }
}
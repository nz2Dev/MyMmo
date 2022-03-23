using MyMmo.Commons.Primitives;
using MyMmo.Commons.Snapshots;

namespace MyMmo.ConsolePlayTest {
    public class PlayTestItem {

        public string ItemId { get; }
        public int LocationId { get; set; }
        public Vector2 PositionInLocation { get; set; }
        
        public PlayTestItem(ItemSnapshotData itemSnapshotData) {
            ItemId = itemSnapshotData.ItemId;
            LocationId = itemSnapshotData.LocationId;
            PositionInLocation = itemSnapshotData.PositionInLocation;
        }
    }
}
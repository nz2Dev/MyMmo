using MyMmo.Commons.Snapshots;
using MyMmo.Processing.Components;
using MyMmo.Processing.Utils;

namespace MyMmo.Processing {
    public class Entity {
        
        public Entity(string id, Transform transform) {
            Id = id;
            Transform = transform;
        }

        public string Id { get; }
        public Transform Transform { get; }
        public Movement Movement { get; } = new Movement();
        
        public ItemSnapshotData GenerateSnapshot() {
            return new ItemSnapshotData {
                ItemId = Id,
                LocationId = Transform.LocationId,
                PositionInLocation = Transform.Position.ToDataVector2()
            };
        }
        
        public void RecordAllChanges(Clip clip) {
            Transform.RecordChanges(Id, clip);
            // other components that can do that, should
        }

    }
}
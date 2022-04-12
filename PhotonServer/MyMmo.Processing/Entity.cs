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
        public Motion Motion { get; } = new Motion();
        public Pathfinder Pathfinder { get; } = new Pathfinder();
        public Wondering Wondering { get; } = new Wondering();
        
        public EntitySnapshotData GenerateSnapshot() {
            return new EntitySnapshotData {
                ItemId = Id,
                PositionInLocation = Transform.Position.ToDataVector2()
            };
        }
        
        public void RecordAllChanges(Clip clip, float deltaTime) {
            Transform.RecordChanges(Id, clip, deltaTime);
        }

    }
}
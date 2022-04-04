using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Processing.Systems;

namespace MyMmo.Processing {
    public class Scene {

        private readonly List<Entity> entities = new List<Entity>();
        private readonly PathfinderSystem pathfinderSystem = new PathfinderSystem();
        private readonly MotionSystem motionSystem = new MotionSystem();
        private readonly List<IUpdate> updates;
        private readonly Clip clip = new Clip();

        public Scene(IEnumerable<Entity> initial, List<IUpdate> updates) {
            this.updates = updates;
            entities.AddRange(initial);
        }
        
        public void RecordSpawnImmediately(Entity entity) {
            entities.Add(entity);
            clip.AddChangesScript(entity.Id, new SpawnItemScriptData {
                ItemSnapshotData = entity.GenerateSnapshot()
            });
        }

        public void RecordDeleteImmediately(string id) {
            var toDelete = entities.FirstOrDefault(entity => entity.Id == id);
            entities.Remove(toDelete);
            clip.AddChangesScript(id, new DestroyItemScriptData {
                ItemId = id
            });
        }

        public Entity GetEntity(string entityId) {
            return entities.FirstOrDefault(entity => entity.Id == entityId);
        }

        public ScriptsClipData Simulate() {
            for (int i = 0; i < 5; i++) {
                foreach (var update in updates) {
                    update.Process(this);
                }
                
                foreach (var entity in entities) {
                    pathfinderSystem.Update(this, entity);
                }
                
                foreach (var entity in entities) {
                    motionSystem.Update(this, entity);
                }

                foreach (var entity in entities) {
                    entity.RecordAllChanges(clip);
                }
            }

            return clip.ToData();
        }

    }
}
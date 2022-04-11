using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Processing.Systems;

namespace MyMmo.Processing {
    public class Scene {

        private readonly List<Entity> entities = new List<Entity>();
        private readonly PathfinderSystem pathfinderSystem = new PathfinderSystem();
        private readonly WonderingSystem wonderingSystem = new WonderingSystem();
        private readonly MotionSystem motionSystem = new MotionSystem();
        private readonly Clip clip = new Clip();

        public IEnumerable<Entity> Entities => entities;

        public Scene(IEnumerable<Entity> initialEntities = null) {
            if (initialEntities != null) {
                entities.AddRange(initialEntities);
            }
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

        // todo: last changes update probably won't be recorded in this way, has to be enqueued and executed after changes recording is done
        public void RecordExitImmediately(string id) {
            var toExit = entities.FirstOrDefault(entity => entity.Id == id);
            entities.Remove(toExit);
            clip.AddChangesScript(id, new ExitItemScriptData {
                ItemId = id
            });
        }

        public void RecordEnterImmediately(Entity entity) {
            entities.Add(entity);
            clip.AddChangesScript(entity.Id, new EnterItemScriptData {
                ItemSnapshotData = entity.GenerateSnapshot()
            });
        }

        public Entity GetEntity(string entityId) {
            return entities.FirstOrDefault(entity => entity.Id == entityId);
        }

        public ScriptsClipData Simulate(IEnumerable<IUpdate> updates, float stepTime, float simulationTime) {
            clip.Rest(stepTime);
            
            var updatesLeft = updates.ToList();
            for (var timePassed = 0f; timePassed < simulationTime && updatesLeft.Count > 0; timePassed += stepTime) {
                updatesLeft.RemoveAll(update => {
                    return update.Process(this, timePassed, simulationTime);
                });
                
                foreach (var entity in entities) {
                    pathfinderSystem.Update(entity);
                }
                
                foreach (var entity in entities) {
                    wonderingSystem.Update(entity);
                }
                
                foreach (var entity in entities) {
                    motionSystem.Update(entity);
                }

                foreach (var entity in entities) {
                    entity.RecordAllChanges(clip, stepTime);
                }
            }

            return clip.ToData();
        }

    }
}
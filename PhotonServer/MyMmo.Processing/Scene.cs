using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using MyMmo.Processing.Systems;

namespace MyMmo.Processing {
    public class Scene {

        private readonly List<Entity> entities = new List<Entity>();
        private readonly PathfinderSystem pathfinderSystem = new PathfinderSystem();
        private readonly WonderingSystem wonderingSystem = new WonderingSystem();
        private readonly MotionSystem motionSystem = new MotionSystem();
        private readonly Clip clip = new Clip();
        private readonly MapRegion mapRegion;
        
        public Scene(IEnumerable<Entity> initialEntities = null, MapRegion mapRegion = null) {
            this.mapRegion = mapRegion ?? new MapRegion(0);
            if (initialEntities != null) {
                entities.AddRange(initialEntities);
            }
        }

        public IEnumerable<Entity> Entities => entities;
        public MapRegion MapRegion => mapRegion;

        public void RecordSpawnImmediately(Entity entity) {
            entities.Add(entity);
            clip.AddChangesScript(entity.Id, new SpawnItemScriptData {
                EntitySnapshotData = entity.GenerateSnapshot()
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
        public void RecordExitImmediately(string id, int locationId) {
            var toExit = entities.FirstOrDefault(entity => entity.Id == id);
            entities.Remove(toExit);
            clip.AddChangesScript(id, new ExitItemScriptData {
                FromLocationId = mapRegion.Id,
                ToLocationId = locationId,
                ItemId = id
            });
        }

        public void RecordEnterImmediately(Entity entity) {
            entities.Add(entity);
            clip.AddChangesScript(entity.Id, new EnterItemScriptData {
                EntitySnapshotData = entity.GenerateSnapshot()
            });
        }

        // todo add time key concept per change, or empty span concept to clip recording.
        public void RecordIdleImmediately(string entityId) {
            clip.AddChangesScript(entityId, new StepIdle());
        }

        public Entity GetEntity(string entityId) {
            return entities.FirstOrDefault(entity => entity.Id == entityId);
        }

        public SceneSnapshotData GenerateSnapshot() {
            return new SceneSnapshotData {
                EntitiesSnapshotData = entities.Select(entity => entity.GenerateSnapshot()).ToArray(),
            };
        }

        public ScriptsClipData Simulate(IEnumerable<IUpdate> updates, float stepTime, float simulationTime) {
            clip.RestartRecord(stepTime);
            
            var updatesLeft = updates.ToList();
            for (var timePassed = 0f; timePassed < simulationTime && updatesLeft.Count > 0; timePassed += stepTime) {
                updatesLeft.RemoveAll(update => update.Process(this, timePassed, simulationTime));

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
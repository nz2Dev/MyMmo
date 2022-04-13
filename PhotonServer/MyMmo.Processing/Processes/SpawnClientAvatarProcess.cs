using MyMmo.Processing.Components;

namespace MyMmo.Processing.Processes {
    
    public class SpawnClientAvatarProcess : IProcess {

        private readonly string itemId;

        public SpawnClientAvatarProcess(string itemId) {
            this.itemId = itemId;
        }

        public bool Process(Scene scene, ProcessTimeContext timeContext) {
            var position = scene.MapRegion.GetRandomPositionWithinBounds();
            scene.RecordSpawnImmediately(new Entity(itemId, new Transform(position)));
            return true;
        }

    }
}
using MyMmo.Processing.Components;

namespace MyMmo.Processing.Updates {
    
    public class SpawnClientAvatarUpdate : IUpdate {

        private readonly string itemId;

        public SpawnClientAvatarUpdate(string itemId) {
            this.itemId = itemId;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            var position = scene.MapRegion.GetRandomPositionWithinBounds();
            scene.RecordSpawnImmediately(new Entity(itemId, new Transform(position)));
            return true;
        }

    }
}
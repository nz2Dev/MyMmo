using MyMmo.Processing;
using MyMmo.Processing.Components;
using MyMmo.Server.Domain;

namespace MyMmo.Server.Updates {
    
    public class SpawnClientAvatarUpdate : BaseServerUpdate {

        private readonly string itemId;

        public SpawnClientAvatarUpdate(string itemId) {
            this.itemId = itemId;
        }

        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            var position = scene.MapRegion.GetRandomPositionWithinBounds();
            scene.RecordSpawnImmediately(new Entity(itemId, new Transform(position)));
            return true;
        }

    }
}
using MyMmo.Server.Scripts;

namespace MyMmo.Server.Producers {
    public class SpawnItemProducer : IScriptProducer<IScript> {

        private readonly string itemId;
        private readonly int locationId;
        private readonly World world;

        public SpawnItemProducer(string itemId, int locationId, World world) {
            this.itemId = itemId;
            this.locationId = locationId;
            this.world = world;
        }

        public IScript ProduceImmediately() {
            var locationArea = world.GetLocationArea(locationId);
            var spawnPosition = locationArea.GetRandomPositionWithinBounds();
            return new SpawnItemScript(itemId, locationId, spawnPosition);
        }

    }
}
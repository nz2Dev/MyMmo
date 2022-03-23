using MyMmo.Server.Scripts;
using Photon.SocketServer;

namespace MyMmo.Server.Producers {
    public class SpawnClientAvatarProducer : IScriptProducer<IScript> {

        public readonly string itemId;
        private readonly int locationId;
        private readonly PeerBase owner;
        private readonly ClientInterestArea interestArea;

        public SpawnClientAvatarProducer(string itemId, int locationId, ClientInterestArea interestArea, PeerBase owner) {
            this.itemId = itemId;
            this.locationId = locationId;
            this.interestArea = interestArea;
            this.owner = owner;
        }

        public bool IsValidAt(World world) {
            return !world.ContainItem(itemId);
        }

        public IScript ProduceImmediately(World world) {
            var locationArea = world.GetLocationArea(locationId);
            var spawnPosition = locationArea.GetRandomPositionWithinBounds();
            return new SpawnItemScript(itemId, locationId, spawnPosition, owner, interestArea);
        }

    }
}
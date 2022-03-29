using MyMmo.Server.Game.Scripts;
using Photon.SocketServer;

namespace MyMmo.Server.Game.Writers {
    public class SpawnClientAvatarWriter : IScriptWriter {

        public readonly string itemId;
        private readonly int locationId;
        private readonly PeerBase owner;
        private readonly ClientInterestArea interestArea;

        public SpawnClientAvatarWriter(string itemId, int locationId, ClientInterestArea interestArea, PeerBase owner) {
            this.itemId = itemId;
            this.locationId = locationId;
            this.interestArea = interestArea;
            this.owner = owner;
        }

        public bool IsValidAt(World world) {
            return !world.ContainItem(itemId);
        }

        public IScript ProduceImmediately(World world) {
            var locationArea = world.GetMapRegion(locationId);
            var spawnPosition = locationArea.GetRandomPositionWithinBounds();
            return new SpawnItemScript(itemId, locationId, spawnPosition, owner, interestArea);
        }

        public void WriteUpdate(World world, LocationScriptsClip clip, float deltaTimeSec) {
            if (clip.TryGetLastItemScriptOf<SpawnItemScript>(itemId, out var lastScript)) {
                if (lastScript.Owner == owner) {
                    return;
                }
            }
            clip.AddItemScript(itemId, ProduceImmediately(world));
        }

    }
}
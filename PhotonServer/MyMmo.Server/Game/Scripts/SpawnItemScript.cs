using System.Numerics;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using MyMmo.Server.Primitives;
using Photon.SocketServer;

namespace MyMmo.Server.Game.Scripts {
    public class SpawnItemScript : IScript {

        private readonly string itemId;
        private readonly int locationId;
        private readonly Vector2 position;
        private readonly PeerBase owner;
        private readonly ClientInterestArea interestArea;

        public SpawnItemScript(string itemId, int locationId, Vector2 position, PeerBase owner,
            ClientInterestArea interestArea) {
            this.itemId = itemId;
            this.locationId = locationId;
            this.position = position;
            this.owner = owner;
            this.interestArea = interestArea;
        }

        public BaseScriptData ToScriptData() {
            return new SpawnItemScriptData {
                ItemSnapshotData = new ItemSnapshotData {
                    ItemId = itemId,
                    LocationId = locationId,
                    PositionInLocation = position.ToDataVector2()
                }
            };
        }

        public void ApplyState(World world) {
            var item = new Item(itemId, owner);
            world.RegisterItem(item);
            item.ChangePositionInLocation(position);
            item.Spawn(locationId);
            interestArea.FollowLocationOf(item);
        }
    }
}
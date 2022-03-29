using System.Numerics;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using MyMmo.Server.Game.Primitives;
using Photon.SocketServer;

namespace MyMmo.Server.Game.Scripts {
    public class SpawnItemScript : IScript {

        public string ItemId { get; }
        public int LocationId { get; }
        public Vector2 Position { get; }
        public PeerBase Owner { get; }
        public ClientInterestArea Area { get; }

        public SpawnItemScript(string itemId, int locationId, Vector2 position, PeerBase owner,
            ClientInterestArea interestArea) {
            this.ItemId = itemId;
            this.LocationId = locationId;
            this.Position = position;
            this.Owner = owner;
            this.Area = interestArea;
        }

        public BaseScriptData ToScriptData() {
            return new SpawnItemScriptData {
                ItemSnapshotData = new ItemSnapshotData {
                    ItemId = ItemId,
                    LocationId = LocationId,
                    PositionInLocation = Position.ToDataVector2()
                }
            };
        }

        public void ApplyState(World world) {
            var item = new Item(ItemId, Owner);
            world.RegisterItem(item);
            item.ChangePositionInLocation(Position);
            item.Spawn(LocationId);
            Area.FollowLocationOf(item);
        }
    }
}
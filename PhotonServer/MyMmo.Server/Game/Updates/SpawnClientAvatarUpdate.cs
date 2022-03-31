using MyMmo.Processing;
using MyMmo.Processing.Components;
using MyMmo.Processing.Utils;
using Photon.SocketServer;

namespace MyMmo.Server.Game.Updates {
    
    public class SpawnClientAvatarUpdate : BaseServerUpdate {

        public readonly string itemId;
        private readonly int locationId;
        private readonly PeerBase owner;
        private readonly ClientInterestArea interestArea;

        public SpawnClientAvatarUpdate(string itemId, int locationId, ClientInterestArea interestArea, PeerBase owner) {
            this.itemId = itemId;
            this.locationId = locationId;
            this.interestArea = interestArea;
            this.owner = owner;
        }

        public bool IsValidAt(World world) {
            return !world.ContainItem(itemId);
        }

        public override void Process(Scene scene) {
            // todo can be created form outside, and will solve problem with interest area referencing
            var item = new Item(itemId, owner);
            
            // spawn position logic, NOTE: we are in Entity/Component/System context, so can use that in our adventages
            // probably this call to world.GetMapRegion() should be part of ECS, and has to be populated before starting simulation
            var position = world.GetMapRegion(locationId).GetRandomPositionWithinBounds();
            // setting position first, so entity factory with have this changes
            // todo spawn update should have pure Server Item object reference, and its initial state data object separately
            // todo and once it's spawned, item will update from that state object, so entity factory will not have extra step
            item.ChangePositionInLocation(position);
            item.Spawn(locationId);

            // scene changes, item entity factory with changes callbacks
            scene.RecordSpawnImmediately(new Entity(item.Id, new Transform(position, item.LocationId, changes => {
                item.ChangePositionInLocation(changes.ToPosition.ToComputeVector());
            })));
            
            world.RegisterItem(item);
            interestArea.FollowLocationOf(item);
        }

    }
}
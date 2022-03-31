using MyMmo.Processing;
using MyMmo.Processing.Components;
using MyMmo.Processing.Utils;

namespace MyMmo.Server.Game.Updates {
    
    public class SpawnClientAvatarUpdate : BaseServerUpdate {

        private readonly Item avatarItem;
        private readonly int locationId;

        public SpawnClientAvatarUpdate(Item avatarItem, int locationId) {
            this.avatarItem = avatarItem;
            this.locationId = locationId;
        }

        public bool IsValidAt(World world) {
            return !world.ContainItem(avatarItem.Id);
        }

        private bool spawned;

        public override void Process(Scene scene) {
            if (spawned) {
                return;
            }

            // spawn position logic, NOTE: we are in Entity/Component/System context, so can use that in our adventages
            // probably this call to world.GetMapRegion() should be part of ECS, and has to be populated before starting simulation
            var position = world.GetMapRegion(locationId).GetRandomPositionWithinBounds();
            // setting position first, so entity factory with have this changes
            // todo spawn update should have pure Server Item object reference, and its initial state data object separately
            // todo and once it's spawned, item will update from that state object, so entity factory will not have extra step
            avatarItem.ChangePositionInLocation(position);
            avatarItem.Spawn(locationId);

            // scene changes, item entity factory with changes callbacks
            scene.RecordSpawnImmediately(new Entity(avatarItem.Id, new Transform(position, avatarItem.LocationId, changes => {
                avatarItem.ChangePositionInLocation(changes.ToPosition.ToComputeVector());
            })));
            world.RegisterItem(avatarItem);
            
            spawned = true;
        }

    }
}
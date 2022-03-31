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
            // this item is never registered at this point, so changing it has no effect, so could be anything
            avatarItem.ChangePositionInLocation(position);
            avatarItem.ChangeLocation(locationId);

            // scene changes, item entity factory with changes callbacks
            scene.RecordSpawnImmediately(new Entity(avatarItem.Id, new Transform(avatarItem.PositionInLocation, avatarItem.LocationId, changes => {
                avatarItem.ChangePositionInLocation(changes.ToPosition.ToComputeVector());
            })));
            // for this call is important to be done in the right time and place,
            // but does it has to be together with scene changes?
            // entity is separated from items, so no state or logic of ECS is affected, but does the ChangesCallbacks 
            world.RegisterItem(avatarItem);
            
            spawned = true;
        }

    }
}
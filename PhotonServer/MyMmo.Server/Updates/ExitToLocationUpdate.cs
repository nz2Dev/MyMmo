using MyMmo.Processing;

namespace MyMmo.Server.Updates {
    public class ExitToLocationUpdate : BaseServerUpdate {

        private string itemId;
        private int newLocationId;

        public ExitToLocationUpdate(string itemId, int newLocationId) {
            this.itemId = itemId;
            this.newLocationId = newLocationId;
        }

        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            var entity = scene.GetEntity(itemId);
            
            var item = world.GetItem(itemId);
            var mapRegion = world.GetMapRegion(item.LocationId);
            entity.Pathfinder.Target = mapRegion.GetExitPositionTo(newLocationId);
            
            var distanceToTarget = (entity.Pathfinder.Target - entity.Transform.Position).Length();
            if (distanceToTarget < 0.1f) {
                item.Transitive = true;
                scene.RecordExitImmediately(entity.Id);
                world.GetLocation(newLocationId).RequestUpdate(new EnterFromLocationUpdate(item, item.LocationId, newLocationId));
                return true;
            }
            
            return false;
        }

    }
}
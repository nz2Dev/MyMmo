using MyMmo.Processing;

namespace MyMmo.Server.Updates {
    public class ExitToLocationUpdate : BaseServerUpdate {

        private readonly string itemId;
        private readonly int newLocationId;

        public ExitToLocationUpdate(string itemId, int newLocationId) {
            this.itemId = itemId;
            this.newLocationId = newLocationId;
        }

        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            var entity = scene.GetEntity(itemId);
            entity.Pathfinder.Target = scene.MapRegion.GetExitPositionTo(newLocationId);
            
            var distanceToTarget = (entity.Pathfinder.Target - entity.Transform.Position).Length();
            if (distanceToTarget < 0.1f) {
                scene.RecordExitImmediately(entity.Id);
                var fromLocationId = world.GetItem(itemId).LocationId;
                world.GetLocation(newLocationId).RequestUpdate(new EnterFromLocationUpdate(itemId, fromLocationId));
                return true;
            }
            
            return false;
        }

    }
}
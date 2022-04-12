namespace MyMmo.Processing.Updates {
    public class ExitToLocationUpdate : IUpdate {

        private readonly string itemId;
        private readonly int locationId;

        public ExitToLocationUpdate(string itemId, int locationId) {
            this.itemId = itemId;
            this.locationId = locationId;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            var entity = scene.GetEntity(itemId);
            entity.Pathfinder.Target = scene.MapRegion.GetExitPositionTo(locationId);
            
            var distanceToTarget = (entity.Pathfinder.Target - entity.Transform.Position).Length();
            if (distanceToTarget < 0.1f) {
                scene.RecordExitImmediately(entity.Id, locationId);
                return true;
            }
            
            return false;
        }

    }
}
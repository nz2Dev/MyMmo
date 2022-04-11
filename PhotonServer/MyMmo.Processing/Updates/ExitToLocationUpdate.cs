namespace MyMmo.Processing.Updates {
    public class ExitToLocationUpdate : IUpdate {

        private readonly string itemId;
        private readonly Scene newScene;

        public ExitToLocationUpdate(string itemId, Scene newScene) {
            this.itemId = itemId;
            this.newScene = newScene;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            var entity = scene.GetEntity(itemId);
            entity.Pathfinder.Target = scene.MapRegion.GetExitPositionTo(newScene.MapRegion.Id);
            
            var distanceToTarget = (entity.Pathfinder.Target - entity.Transform.Position).Length();
            if (distanceToTarget < 0.1f) {
                var fromMapRegionId = scene.MapRegion.Id;
                scene.RecordExitImmediately(entity.Id);
                newScene.BufferUpdate(new EnterFromLocationUpdate(itemId, fromMapRegionId));
                return true;
            }
            
            return false;
        }

    }
}
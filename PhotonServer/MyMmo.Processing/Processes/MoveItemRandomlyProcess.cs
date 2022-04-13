namespace MyMmo.Processing.Processes {
    public class MoveItemRandomlyProcess : IProcess {

        private readonly string sourceItemId;

        public MoveItemRandomlyProcess(string sourceItemId) {
            this.sourceItemId = sourceItemId;
        }
        
        public bool Process(Scene scene, ProcessTimeContext timeContext) {
            var entity = scene.GetEntity(sourceItemId);
            
            if (entity.Pathfinder.Target == default) {
                entity.Pathfinder.Target = scene.MapRegion.GetRandomPositionWithinBounds();
                return false; // false = it's not done yet, todo flip
            } else {
                var distanceToTarget = (entity.Pathfinder.Target - entity.Transform.Position).Length();
                
                var isAtTarget = distanceToTarget < 0.1f;
                if (isAtTarget) {
                    // because now scene state is persistent after simulation, we have to reset some logic components state
                    entity.Pathfinder.Target = default;
                }

                // keep alive until at target destination
                return isAtTarget;
            }
        }
    }
}
using MyMmo.Processing;

namespace MyMmo.Server.Updates {
    public class MoveItemRandomlyUpdate : BaseServerUpdate {

        private readonly string sourceItemId;

        public MoveItemRandomlyUpdate(string sourceItemId) {
            this.sourceItemId = sourceItemId;
        }
        
        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            var item = world.GetItem(sourceItemId);
            var mapRegion = world.GetMapRegion(item.LocationId);
            var entity = scene.GetEntity(sourceItemId);
            
            if (entity.Pathfinder.Target == default) {
                entity.Pathfinder.Target = mapRegion.GetRandomPositionWithinBounds();
                return false;
            } else {
                var distanceToTarget = (entity.Pathfinder.Target - entity.Transform.Position).Length();
                // keep alive until at target destination
                return distanceToTarget < 0.1f;
            }
        }
    }
}
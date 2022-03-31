using MyMmo.Processing;

namespace MyMmo.Server.Game.Updates {
    public class MoveItemRandomlyUpdate : BaseServerUpdate {

        private readonly string sourceItemId;

        public MoveItemRandomlyUpdate(string sourceItemId) {
            this.sourceItemId = sourceItemId;
        }

        private bool targetIsSet;
        
        public override void Process(Scene scene) {
            if (targetIsSet) {
                return;
            }
            
            var item = world.GetItem(sourceItemId);
            var mapRegion = world.GetMapRegion(item.LocationId);
            scene.GetEntity(sourceItemId).Movement.Target = mapRegion.GetRandomPositionWithinBounds();
            targetIsSet = true;
        }
    }
}
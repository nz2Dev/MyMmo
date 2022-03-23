using MyMmo.Server.Scripts;

namespace MyMmo.Server.Producers {
    public class MoveItemRandomlyProducer : IScriptProducer<ChangePositionScript> {

        private readonly string sourceItemId;

        public MoveItemRandomlyProducer(string sourceItemId) {
            this.sourceItemId = sourceItemId;
        }

        public ChangePositionScript ProduceImmediately(World world) {
            var sourceItem = world.GetItem(sourceItemId);
            var locationArea = world.GetLocationArea(sourceItem.LocationId);
            var newPosition = locationArea.GetRandomPositionWithinBounds();
            
            return new ChangePositionScript(
                sourceItemId,
                fromPosition: sourceItem.PositionInLocation,
                toPosition: newPosition
            );
        }

    }
}
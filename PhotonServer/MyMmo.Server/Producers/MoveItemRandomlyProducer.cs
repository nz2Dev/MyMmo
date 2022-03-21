using System;
using MyMmo.Server.Scripts;

namespace MyMmo.Server.Producers {
    public class MoveItemRandomlyProducer : IScriptProducer<ChangePositionScript> {

        private readonly World world;
        private readonly string sourceItemId;

        public MoveItemRandomlyProducer(World world, string sourceItemId) {
            this.world = world;
            this.sourceItemId = sourceItemId;
        }

        public ChangePositionScript ProduceImmediately() {
            if (!world.TryGetItem(sourceItemId, out var sourceItem)) {
                throw new Exception("Item not found: " + sourceItemId);
            }

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
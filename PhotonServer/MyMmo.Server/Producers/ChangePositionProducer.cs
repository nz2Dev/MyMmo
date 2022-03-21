using System;
using MyMmo.Server.Scripts;

namespace MyMmo.Server.Producers {
    public class ChangePositionProducer : IScriptProducer<ChangePositionScript> {

        private readonly World world;
        private readonly string sourceItemId;

        public ChangePositionProducer(World world, string sourceItemId) {
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
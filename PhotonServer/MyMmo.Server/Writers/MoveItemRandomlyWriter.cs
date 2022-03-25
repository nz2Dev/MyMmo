using MyMmo.Server.Scripts;

namespace MyMmo.Server.Writers {
    public class MoveItemRandomlyWriter : IScriptWriter {

        private readonly string sourceItemId;

        public MoveItemRandomlyWriter(string sourceItemId) {
            this.sourceItemId = sourceItemId;
        }

        public IScript ProduceImmediately(World world) {
            var sourceItem = world.GetItem(sourceItemId);
            var locationArea = world.GetLocationArea(sourceItem.LocationId);
            var newPosition = locationArea.GetRandomPositionWithinBounds();
            
            return new ChangePositionScript(
                sourceItemId,
                fromPosition: sourceItem.PositionInLocation,
                toPosition: newPosition
            );
        }

        public void Write(World world, ScriptsClip clip) {
            clip.SetItemScript(sourceItemId, ProduceImmediately(world));
        }

    }
}
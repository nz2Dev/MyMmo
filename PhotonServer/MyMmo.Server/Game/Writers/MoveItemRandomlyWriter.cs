using System.Numerics;
using MyMmo.Server.Game.Primitives;
using MyMmo.Server.Game.Scripts;

namespace MyMmo.Server.Game.Writers {
    public class MoveItemRandomlyWriter : IScriptWriter {

        private readonly string sourceItemId;

        public MoveItemRandomlyWriter(string sourceItemId) {
            this.sourceItemId = sourceItemId;
        }

        public IScript ProduceImmediately(World world) {
            var sourceItem = world.GetItem(sourceItemId);
            var locationArea = world.GetMapRegion(sourceItem.LocationId);
            var newPosition = locationArea.GetRandomPositionWithinBounds();
            
            return new ChangePositionScript(
                sourceItemId,
                duration: -1f,
                trajectory: new Line {
                    pointA = sourceItem.PositionInLocation,
                    pointB = newPosition
                },
                destination: newPosition,
                finishing: true
            );
        }

        public void WriteUpdate(World world, LocationScriptsClip clip, float deltaTimeSec) {
            var sourceItem = world.GetItem(sourceItemId);
            
            Vector2 targetPosition;
            Vector2 lastPosition;
            if (clip.TryGetLastItemScriptOf<ChangePositionScript>(sourceItemId, out var lastScript)) {
                if (lastScript.Finishing) {
                    return;
                }
                targetPosition = lastScript.Destination;
                lastPosition = lastScript.Trajectory.pointB;
            } else {
                var locationMapRegion = world.GetMapRegion(sourceItem.LocationId);
                targetPosition = locationMapRegion.GetRandomPositionWithinBounds();
                lastPosition = sourceItem.PositionInLocation;
            }

            var moveVector = targetPosition - lastPosition;
            var maxUnitsDelta = sourceItem.MovementSpeedUnitsPerSecond * deltaTimeSec;

            var finishing = true;
            var resultMoveVector = moveVector;
            if (moveVector.Length() > maxUnitsDelta) {
                resultMoveVector = maxUnitsDelta * Vector2.Normalize(moveVector);
                finishing = false;
            }

            clip.AddItemScript(sourceItemId, new ChangePositionScript(
                sourceItemId,
                duration: deltaTimeSec,
                destination: targetPosition,
                finishing: finishing,
                trajectory: new Line {
                    pointA = lastPosition,
                    pointB = lastPosition + resultMoveVector
                }
            ));
        }
    }
}
using System.Linq;
using System.Numerics;
using MyMmo.Server.Game.Scripts;
using MyMmo.Server.Primitives;

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
                trajectoryLine: new Line {
                    pointA = sourceItem.PositionInLocation,
                    pointB = newPosition
                }
            );
        }

        public void Write(World world, LocationScriptsClip clip) {
            clip.SetItemScript(sourceItemId, ProduceImmediately(world));
        }

        public void WriteIntent(World world, LocationScriptsClip clip, float timeSpanSec) {
            var sourceItem = world.GetItem(sourceItemId);
            var locationArea = world.GetMapRegion(sourceItem.LocationId);
            var targetPosition = locationArea.GetRandomPositionWithinBounds();
            
            var wantedMovementVector = targetPosition - sourceItem.PositionInLocation;
            var neededMovementDuration = wantedMovementVector.Length() * sourceItem.MovementSpeedUnitsPerSecond;
            var availableMovementDuration = System.Math.Min(neededMovementDuration, timeSpanSec);
            var movementVector = Vector2.Normalize(wantedMovementVector) * availableMovementDuration;
            
            clip.SetItemScriptIntent(sourceItemId, new ChangePositionScript(
                itemId: sourceItemId,
                duration: availableMovementDuration,
                trajectoryLine: new Line {
                    pointA = sourceItem.PositionInLocation,
                    pointB = sourceItem.PositionInLocation + movementVector
                }
            ));
        }

        public void ProcessLastIntents(World world, LocationScriptsClip clip) {
            var ourLastScriptIntent = (ChangePositionScript) clip.GetItemLastScriptIntent(sourceItemId);
            // we try to find other intents that might affect our intent behaviour
            var otherChangePositionIntents = clip.LastIntents().OfType<ChangePositionScript>().Except(ourLastScriptIntent.ToEnumerable());

            var obstaclesAgents = otherChangePositionIntents.Select(script => new ChangePositionScriptAgent(script));
            if (Navigation.TryAvoid(obstaclesAgents, new ChangePositionScriptAgent(ourLastScriptIntent), out var newTrajectory)) {
                // when we found one, we change our intent
                ourLastScriptIntent.TrajectoryLine = newTrajectory;
            }
        }
    }
    
    public class ChangePositionScriptAgent : Navigation.IAgent {

        private readonly ChangePositionScript script;

        public ChangePositionScriptAgent(ChangePositionScript script) {
            this.script = script;
        }

        public Line Trajectory => script.TrajectoryLine;
        public float Radius => 1;
            
    }
}
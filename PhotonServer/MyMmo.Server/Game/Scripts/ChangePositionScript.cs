using System.Numerics;
using MyMmo.Commons.Scripts;
using MyMmo.Server.Primitives;

namespace MyMmo.Server.Game.Scripts {
    public class ChangePositionScript : IScript {

        public string ItemId { get; }
        public Line Trajectory { get; }
        public float Duration { get; }
        public Vector2 Destination { get; }
        public bool Finishing { get; }

        public ChangePositionScript(string itemId, Line trajectory, float duration, Vector2 destination, bool finishing) {
            ItemId = itemId;
            Trajectory = trajectory;
            Duration = duration;
            Destination = destination;
            Finishing = finishing;
        }

        public BaseScriptData ToScriptData() {
            return new ChangePositionScriptData {
                FromPosition = Trajectory.pointA.ToDataVector2(),
                ToPosition = Trajectory.pointB.ToDataVector2(),
                Destination = Destination.ToDataVector2(),
                Duration = Duration,
                ItemId = ItemId
            };
        }

        public void ApplyState(World world) {
            var item = world.GetItem(ItemId);
            item.ChangePositionInLocation(Trajectory.pointB);
        }

    }
}
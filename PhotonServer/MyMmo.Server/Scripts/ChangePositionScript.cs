using MyMmo.Commons.Scripts;
using MyMmo.Server.Math;

namespace MyMmo.Server.Scripts {
    public class ChangePositionScript : IScript {

        public string ItemId { get; set; }
        public Line TrajectoryLine { get; set; }
        public float Duration { get; set; }

        public ChangePositionScript(string itemId, Line trajectoryLine, float duration) {
            ItemId = itemId;
            TrajectoryLine = trajectoryLine;
            Duration = duration;
        }

        public BaseScriptData ToScriptData() {
            return new ChangePositionScriptData {
                FromPosition = TrajectoryLine.pointA.ToDataVector2(),
                ToPosition = TrajectoryLine.pointB.ToDataVector2(),
                ItemId = ItemId
            };
        }

        public void ApplyState(World world) {
            var item = world.GetItem(ItemId);
            item.ChangePositionInLocation(TrajectoryLine.pointB);
        }

    }
}
using MyMmo.Processing.Components;

namespace MyMmo.Processing.Processes {
    public class EnterFromLocationProcess : IProcess {

        private readonly string itemId;
        private readonly int fromMapRegionId;
        private readonly float absoluteEnterTime;

        public EnterFromLocationProcess(string itemId, int fromMapRegionId, float absoluteEnterTime) {
            this.itemId = itemId;
            this.fromMapRegionId = fromMapRegionId;
            this.absoluteEnterTime = absoluteEnterTime;
        }

        public bool Process(Scene scene, ProcessTimeContext timeContext) {
            if (timeContext.AbsoluteTimePassed() < absoluteEnterTime) {
                // workaround current timing concept,
                // so EnterChangeScript won't be placed at the beginning
                scene.RecordIdleImmediately(itemId);
                return false;
            }

            var enterPosition = scene.MapRegion.GetEnterPointFrom(fromMapRegionId);
            scene.RecordEnterImmediately(new Entity(itemId, new Transform(enterPosition)));
            return true;
        }

    }
}
using MyMmo.Processing.Components;

namespace MyMmo.Processing.Updates {
    public class EnterFromLocationUpdate : IUpdate {

        private readonly string itemId;
        private readonly int fromMapRegionId;
        private readonly float enterTime;

        public EnterFromLocationUpdate(string itemId, int fromMapRegionId, float enterTime) {
            this.itemId = itemId;
            this.fromMapRegionId = fromMapRegionId;
            this.enterTime = enterTime;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            if (timePassed < enterTime) {
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
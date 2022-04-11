using MyMmo.Processing.Components;

namespace MyMmo.Processing.Updates {
    public class EnterFromLocationUpdate : IUpdate {

        private readonly string itemId;
        private readonly int fromMapRegionId;

        public EnterFromLocationUpdate(string itemId, int fromMapRegionId) {
            this.itemId = itemId;
            this.fromMapRegionId = fromMapRegionId;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            var enterPosition = scene.MapRegion.GetEnterPointFrom(fromMapRegionId);
            scene.RecordEnterImmediately(new Entity(itemId, new Transform(enterPosition)));
            return true;
        }

    }
}
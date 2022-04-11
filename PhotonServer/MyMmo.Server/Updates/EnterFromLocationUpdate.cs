using MyMmo.Processing;
using MyMmo.Processing.Components;

namespace MyMmo.Server.Updates {
    public class EnterFromLocationUpdate : BaseServerUpdate {

        private readonly string itemId;
        private readonly int fromLocationId;

        public EnterFromLocationUpdate(string itemId, int fromLocationId) {
            this.itemId = itemId;
            this.fromLocationId = fromLocationId;
        }

        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            var enterPosition = scene.MapRegion.GetEnterPointFrom(fromLocationId);
            scene.RecordEnterImmediately(new Entity(itemId, new Transform(enterPosition)));
            return true;
        }

    }
}
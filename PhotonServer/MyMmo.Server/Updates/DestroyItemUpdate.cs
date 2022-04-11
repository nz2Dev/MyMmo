using MyMmo.Processing;

namespace MyMmo.Server.Updates {
    public class DestroyItemUpdate : BaseServerUpdate {

        private readonly string itemId;

        public DestroyItemUpdate(string itemId) {
            this.itemId = itemId;
        }

        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            scene.RecordDeleteImmediately(itemId);
            return true;
        }

    }
}
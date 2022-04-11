namespace MyMmo.Processing.Updates {
    public class DestroyItemUpdate : IUpdate {

        private readonly string itemId;

        public DestroyItemUpdate(string itemId) {
            this.itemId = itemId;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            scene.RecordDeleteImmediately(itemId);
            return true;
        }

    }
}
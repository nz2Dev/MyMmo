namespace MyMmo.Processing.Processes {
    public class DestroyItemProcess : IProcess {

        private readonly string itemId;

        public DestroyItemProcess(string itemId) {
            this.itemId = itemId;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            scene.RecordDeleteImmediately(itemId);
            return true;
        }

    }
}
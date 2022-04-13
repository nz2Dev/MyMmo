namespace MyMmo.Processing.Processes {
    public class DestroyItemProcess : IProcess {

        private readonly string itemId;

        public DestroyItemProcess(string itemId) {
            this.itemId = itemId;
        }

        public bool Process(Scene scene, ProcessTimeContext timeContext) {
            scene.RecordDeleteImmediately(itemId);
            return true;
        }

    }
}
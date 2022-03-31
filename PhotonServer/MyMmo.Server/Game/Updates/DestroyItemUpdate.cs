using MyMmo.Processing;

namespace MyMmo.Server.Game.Updates {
    public class DestroyItemUpdate : BaseServerUpdate {

        private readonly string itemId;

        public DestroyItemUpdate(string itemId) {
            this.itemId = itemId;
        }

        public override void Process(Scene scene) {
            scene.RecordDeleteImmediately(itemId);
            var item = world.GetItem(itemId);
            world.RemoveItem(item);
            item.Destroy();
            item.Dispose();
        }

    }
}
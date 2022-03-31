using MyMmo.Processing;

namespace MyMmo.Server.Updates {
    public class DestroyItemUpdate : BaseServerUpdate {

        private readonly string itemId;

        public DestroyItemUpdate(string itemId) {
            this.itemId = itemId;
        }

        private bool destroyed;
        
        public override void Process(Scene scene) {
            if (destroyed) {
                return;
            }
            
            scene.RecordDeleteImmediately(itemId);
            var item = world.GetItem(itemId);
            world.RemoveItem(item);
            item.Destroy();
            item.Dispose();
            destroyed = true;
        }

    }
}
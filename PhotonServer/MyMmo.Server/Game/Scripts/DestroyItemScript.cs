using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Game.Scripts {
    public class DestroyItemScript : IScript {

        private readonly string itemId;

        public DestroyItemScript(string itemId) {
            this.itemId = itemId;
        }

        public BaseScriptData ToScriptData() {
            return new DestroyItemScriptData {
                ItemId = itemId
            };
        }

        public void ApplyState(World world) {
            var item = world.GetItem(itemId);
            world.RemoveItem(item);
            item.Destroy();
            item.Dispose();
        }

    }
}
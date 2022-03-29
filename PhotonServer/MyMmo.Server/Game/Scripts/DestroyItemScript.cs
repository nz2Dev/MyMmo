using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Game.Scripts {
    public class DestroyItemScript : IScript {

        public string ItemId { get; }

        public DestroyItemScript(string itemId) {
            ItemId = itemId;
        }

        public BaseScriptData ToScriptData() {
            return new DestroyItemScriptData {
                ItemId = ItemId
            };
        }

        public void ApplyState(World world) {
            var item = world.GetItem(ItemId);
            world.RemoveItem(item);
            item.Destroy();
            item.Dispose();
        }

    }
}
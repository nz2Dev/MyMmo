using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Game.Scripts {
    
    public class ChangeLocationScript : IScript {

        public string ItemId { get; }
        public int FromLocationId { get; }
        public int ToLocationId { get; }

        public ChangeLocationScript(string itemId, int fromLocationId, int toLocationId) {
            this.ItemId = itemId;
            this.FromLocationId = fromLocationId;
            this.ToLocationId = toLocationId;
        }

        public BaseScriptData ToScriptData() {
            return new ChangeLocationScriptData {
                FromLocation = FromLocationId,
                ToLocation = ToLocationId,
                ItemId = ItemId
            };
        }

        public void ApplyState(World world) {
            var item = world.GetItem(ItemId);
            item.ChangeLocation(ToLocationId);
        }

    }
}
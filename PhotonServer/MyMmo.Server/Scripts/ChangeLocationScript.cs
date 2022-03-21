using System;
using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Scripts {
    
    public class ChangeLocationScript : IScript {

        private string itemId;
        private int fromLocationId;
        private int toLocationId;

        public ChangeLocationScript(string itemId, int fromLocationId, int toLocationId) {
            this.itemId = itemId;
            this.fromLocationId = fromLocationId;
            this.toLocationId = toLocationId;
        }

        public BaseScriptData ToScriptData() {
            return new ChangeLocationScriptData {
                FromLocation = fromLocationId,
                ToLocation = toLocationId,
                ItemId = itemId
            };
        }

        public void ApplyState(World world) {
            if (!world.TryGetItem(itemId, out var item)) {
                throw new Exception("item from script not found: " + itemId);
            }         
            
            item.ChangeLocation(toLocationId);
        }

    }
}
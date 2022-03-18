using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.Client.Consumers {
    public class ChangeLocationClientScriptReader {

        private readonly Dictionary<string, Item> itemCache;

        public ChangeLocationClientScriptReader(Dictionary<string, Item> itemCache) {
            this.itemCache = itemCache;
        }

        public void ApplyScript(ChangeLocationScript script) {
            if (itemCache.TryGetValue(script.ItemId, out var item)) {
                item.LocationId = script.ToLocation;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.Client.Scripts {
    
    public class ChangeLocationClientScript : IClientScript {

        private readonly ChangeLocationScriptData scriptData;
        
        public ChangeLocationClientScript(ChangeLocationScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void ApplyClientState(Dictionary<string, Item> itemCache) {
            if (!itemCache.TryGetValue(scriptData.ItemId, out var item)) {
                throw new Exception("client item not found: " + scriptData.ItemId);
            }
            
            item.LocationId = scriptData.ToLocation;
        }

    }
}
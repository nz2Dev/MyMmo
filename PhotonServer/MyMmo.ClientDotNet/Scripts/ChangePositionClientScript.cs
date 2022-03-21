using System;
using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.Client.Scripts {
    public class ChangePositionClientScript : IClientScript {

        private readonly ChangePositionScriptData scriptData;

        public ChangePositionClientScript(ChangePositionScriptData changePositionScriptData) {
            scriptData = changePositionScriptData;
        }

        public void ApplyClientState(Dictionary<string, Item> itemCache) {
            if (!itemCache.TryGetValue(scriptData.ItemId, out var item)) {
                throw new Exception("client item not found: " + scriptData.ItemId);
            }

            item.PositionInLocation = scriptData.ToPosition;
        }

    }
}
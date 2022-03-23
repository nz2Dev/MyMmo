using System;
using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    
    public class ChangeLocationConsoleUpdateScript : IConsoleUpdateScript {

        private readonly ChangeLocationScriptData scriptData;
        
        public ChangeLocationConsoleUpdateScript(ChangeLocationScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void ApplyClientState(Dictionary<string, PlayTestItem> itemCache) {
            if (!itemCache.TryGetValue(scriptData.ItemId, out var item)) {
                throw new Exception("client item not found: " + scriptData.ItemId);
            }
            
            ConsolePlayTest.PrintLog($"item {scriptData.ItemId} changes location from {scriptData.FromLocation} to {scriptData.ToLocation}");

            item.LocationId = scriptData.ToLocation;
        }

    }
}
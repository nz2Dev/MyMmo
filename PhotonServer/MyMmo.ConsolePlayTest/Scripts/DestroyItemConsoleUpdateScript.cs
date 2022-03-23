using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public class DestroyItemConsoleUpdateScript : IConsoleUpdateScript {

        private readonly DestroyItemScriptData scriptData;

        public DestroyItemConsoleUpdateScript(DestroyItemScriptData scriptData) {
            this.scriptData = scriptData;
        }
        
        public void ApplyClientState(Dictionary<string, PlayTestItem> itemCache) {
            ConsolePlayTest.PrintLog($"item {scriptData.ItemId} destroyed");
            itemCache.Remove(scriptData.ItemId);
        }

    }
}
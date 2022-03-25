using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public class DestroyItemConsoleScriptApplier : IConsoleScriptApplier {

        private readonly DestroyItemScriptData scriptData;

        public DestroyItemConsoleScriptApplier(DestroyItemScriptData scriptData) {
            this.scriptData = scriptData;
        }
        
        public void ApplyClientState(Dictionary<string, ConsoleItem> itemCache) {
            ConsolePlayTest.PrintLog($"item {scriptData.ItemId} destroyed");
            itemCache.Remove(scriptData.ItemId);
        }

    }
}
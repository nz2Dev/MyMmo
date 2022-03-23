using System;
using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public class ChangePositionConsoleUpdateScript : IConsoleUpdateScript {

        private readonly ChangePositionScriptData scriptData;

        public ChangePositionConsoleUpdateScript(ChangePositionScriptData changePositionScriptData) {
            scriptData = changePositionScriptData;
        }

        public void ApplyClientState(Dictionary<string, PlayTestItem> itemCache) {
            if (!itemCache.TryGetValue(scriptData.ItemId, out var item)) {
                throw new Exception("client item not found: " + scriptData.ItemId);
            }

            ConsolePlayTest.PrintLog($"item {scriptData.ItemId} changes its position to {scriptData.ToPosition}");
            item.PositionInLocation = scriptData.ToPosition;
        }

    }
}
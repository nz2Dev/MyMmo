using System;
using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public class ChangePositionConsoleScriptApplier : IConsoleScriptApplier {

        private readonly ChangePositionScriptData scriptData;

        public ChangePositionConsoleScriptApplier(ChangePositionScriptData changePositionScriptData) {
            scriptData = changePositionScriptData;
        }

        public void ApplyClientState(int locationId, Dictionary<string, ConsoleItem> itemCache) {
            if (!itemCache.TryGetValue(scriptData.ItemId, out var item)) {
                throw new Exception("client item not found: " + scriptData.ItemId);
            }

            ConsolePlayTest.PrintLog($"item {scriptData.ItemId} changes its position to {scriptData.ToPosition}");
            item.PositionInLocation = scriptData.ToPosition;
        }

    }
}
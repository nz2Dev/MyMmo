using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public class EnterItemConsoleScriptApplier : IConsoleScriptApplier {

        private readonly EnterItemScriptData enterItemScriptData;

        public EnterItemConsoleScriptApplier(EnterItemScriptData enterItemScriptData) {
            this.enterItemScriptData = enterItemScriptData;
        }

        public void ApplyClientState(int locationId, Dictionary<string, ConsoleItem> itemCache) {
            var consoleItem = new ConsoleItem(locationId, enterItemScriptData.EntitySnapshotData);
            itemCache[enterItemScriptData.EntitySnapshotData.ItemId] = consoleItem;
            ConsolePlayTest.PrintLog($"item {consoleItem.ItemId} enters location {consoleItem.LocationId} at position {consoleItem.PositionInLocation}");
        }

    }
}
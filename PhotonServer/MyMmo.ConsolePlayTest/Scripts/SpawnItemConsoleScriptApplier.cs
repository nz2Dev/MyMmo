using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public class SpawnItemConsoleScriptApplier : IConsoleScriptApplier {

        private readonly SpawnItemScriptData scriptData;

        public SpawnItemConsoleScriptApplier(SpawnItemScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void ApplyClientState(int locationId, Dictionary<string, ConsoleItem> itemCache) {
            var itemSnapshot = scriptData.EntitySnapshotData;
            ConsolePlayTest.PrintLog($"item {itemSnapshot.ItemId} spawned at location {locationId}");
            itemCache.Add(itemSnapshot.ItemId, new ConsoleItem(locationId, itemSnapshot));
        }

    }
}
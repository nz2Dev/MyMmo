using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public class SpawnItemConsoleUpdateScript : IConsoleUpdateScript {

        private readonly SpawnItemScriptData scriptData;

        public SpawnItemConsoleUpdateScript(SpawnItemScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void ApplyClientState(Dictionary<string, PlayTestItem> itemCache) {
            var itemSnapshot = scriptData.ItemSnapshotData;
            ConsolePlayTest.PrintLog($"item {itemSnapshot.ItemId} spawned at location {itemSnapshot.LocationId}");
            itemCache.Add(itemSnapshot.ItemId, new PlayTestItem(itemSnapshot));
        }

    }
}
using System;
using System.Collections.Generic;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public static class ConsoleScriptApplierFactory {
        
        public static IConsoleScriptApplier Create(BaseScriptData baseScriptData) {
            if (baseScriptData is ChangePositionScriptData changePositionScriptData) {
                return new ChangePositionConsoleScriptApplier(changePositionScriptData);
            } 
            if (baseScriptData is SpawnItemScriptData spawnItemScriptData) {
                return new SpawnItemConsoleScriptApplier(spawnItemScriptData);
            } 
            if (baseScriptData is DestroyItemScriptData destroyItemScriptData) {
                return new DestroyItemConsoleScriptApplier(destroyItemScriptData);
            }

            if (baseScriptData is EnterItemScriptData enterItemScriptData) {
                return new EnterItemConsoleScriptApplier(enterItemScriptData);
            }

            if (baseScriptData is ExitItemScriptData exitItemScriptData) {
                return new ExitItemConsoleScriptApplier(exitItemScriptData);
            }
            
            throw new Exception("No factory found for scriptData: " + baseScriptData);
        }

    }

    public class ExitItemConsoleScriptApplier : IConsoleScriptApplier {

        private readonly ExitItemScriptData exitItemScriptData;

        public ExitItemConsoleScriptApplier(ExitItemScriptData exitItemScriptData) {
            this.exitItemScriptData = exitItemScriptData;
        }

        public void ApplyClientState(Dictionary<string, ConsoleItem> itemCache) {
            var consoleItem = itemCache[exitItemScriptData.ItemId];
            ConsolePlayTest.PrintLog($"item {consoleItem} exits its current location {consoleItem.LocationId} and become transitive");
        }

    }
}
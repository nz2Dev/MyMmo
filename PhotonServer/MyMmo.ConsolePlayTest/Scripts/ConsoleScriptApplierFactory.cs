using System;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public static class ConsoleScriptApplierFactory {
        
        public static IConsoleScriptApplier Create(BaseScriptData baseScriptData) {
            if (baseScriptData is ChangeLocationScriptData changeLocationScriptData) {
                return new ChangeLocationConsoleScriptApplier(changeLocationScriptData);
            } else if (baseScriptData is ChangePositionScriptData changePositionScriptData) {
                return new ChangePositionConsoleScriptApplier(changePositionScriptData);
            } else if (baseScriptData is SpawnItemScriptData spawnItemScriptData) {
                return new SpawnItemConsoleScriptApplier(spawnItemScriptData);
            } else if (baseScriptData is DestroyItemScriptData destroyItemScriptData) {
                return new DestroyItemConsoleScriptApplier(destroyItemScriptData);
            } else {
                throw new Exception("No factory found for scriptData: " + baseScriptData);
            }
        }

    }
}
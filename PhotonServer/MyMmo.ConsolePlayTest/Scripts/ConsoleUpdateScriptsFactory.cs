using System;
using MyMmo.Commons.Scripts;

namespace MyMmo.ConsolePlayTest.Scripts {
    public static class ClientScriptsFactory {
        
        public static IConsoleUpdateScript Create(BaseScriptData baseScriptData) {
            if (baseScriptData is ChangeLocationScriptData changeLocationScriptData) {
                return new ChangeLocationConsoleUpdateScript(changeLocationScriptData);
            } else if (baseScriptData is ChangePositionScriptData changePositionScriptData) {
                return new ChangePositionConsoleUpdateScript(changePositionScriptData);
            } else if (baseScriptData is SpawnItemScriptData spawnItemScriptData) {
                return new SpawnItemConsoleUpdateScript(spawnItemScriptData);
            } else if (baseScriptData is DestroyItemScriptData destroyItemScriptData) {
                return new DestroyItemConsoleUpdateScript(destroyItemScriptData);
            } else {
                throw new Exception("No factory found for scriptData: " + baseScriptData);
            }
        }

    }
}
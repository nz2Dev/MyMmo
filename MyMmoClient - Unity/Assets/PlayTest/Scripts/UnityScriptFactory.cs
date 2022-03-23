
using System;
using MyMmo.Commons.Scripts;

public static class UnityScriptFactory {

    public static IUnityScript Create(BaseScriptData data) {
        if (data is ChangeLocationScriptData changeLocationScriptData) {
            return new ChangeLocationUnityScript(changeLocationScriptData);
        } else if (data is ChangePositionScriptData changePositionScriptData) {
            return new ChangePositionUnityScript(changePositionScriptData);
        } else if (data is SpawnItemScriptData spawnItemScriptData) {
            return new SpawnItemUnityScript(spawnItemScriptData);
        } else if (data is DestroyItemScriptData destroyItemScriptData) {
            return new DestroyItemUnityScript(destroyItemScriptData);
        } else {
            throw new Exception($"Unity script is not implemented for script data: " + data);
        }
    }
}
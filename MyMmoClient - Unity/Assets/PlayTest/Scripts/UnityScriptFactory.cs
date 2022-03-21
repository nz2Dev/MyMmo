
using System;
using MyMmo.Commons.Scripts;

public static class UnityScriptFactory {

    public static IUnityScript Create(BaseScriptData data) {
        if (data is ChangeLocationScriptData changeLocationScriptData) {
            return new ChangeLocationUnityScript(changeLocationScriptData);
        } else if (data is ChangePositionScriptData changePositionScriptData) {
            return new ChangePositionUnityScript(changePositionScriptData);
        } else {
            throw new Exception($"Unity script is not implemented for script data: " + data);
        }
    }
}
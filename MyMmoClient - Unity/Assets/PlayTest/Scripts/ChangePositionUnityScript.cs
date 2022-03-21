using MyMmo.Commons.Scripts;
using UnityEngine;

public class ChangePositionUnityScript : IUnityScript {

    private readonly ChangePositionScriptData scriptData;

    public ChangePositionUnityScript(ChangePositionScriptData scriptData) {
        this.scriptData = scriptData;
    }

    public bool UpdateUnityState() {
        Debug.LogWarning("Change position unity script is not implemented");
        return false;
    }

}
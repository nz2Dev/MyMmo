using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using Object = UnityEngine.Object;

public class DestroyItemUnityScript : IUnityScript {

    private readonly DestroyItemScriptData scriptData;

    public DestroyItemUnityScript(DestroyItemScriptData scriptData) {
        this.scriptData = scriptData;
    }
    
    public bool UpdateUnityState() {
        var targetItem = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(item => item.state.ItemId == scriptData.ItemId);
        if (targetItem == null) {
            throw new Exception($"target item {scriptData.ItemId} not found");
        }
        Object.Destroy(targetItem.gameObject);
        return false;
    }

}
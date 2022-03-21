using System;
using System.Linq;
using MyMmo.Client;
using MyMmo.Commons.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

public class ChangeLocationUnityScript : IUnityScript {

    private readonly Game game;
    private readonly ChangeLocationScriptData scriptData;

    private AvatarItem targetAvatar;
    private Location targetLocation;

    public ChangeLocationUnityScript(ChangeLocationScriptData scriptData) {
        this.scriptData = scriptData;
        
        targetAvatar = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(avatar => avatar.source.Id == scriptData.ItemId);
        if (targetAvatar == null) {
            throw new Exception("can't find script target item with id: " + scriptData.ItemId);
        }

        targetLocation = Object.FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == scriptData.ToLocation);
        if (targetLocation == null) {
            throw new Exception("can't find script's target location with id: " + scriptData.ToLocation);
        }
    }
    
    public bool UpdateUnityState() {
        targetAvatar.transform.position =
            Vector3.Lerp(targetAvatar.transform.position, targetLocation.transform.position, Time.deltaTime * 1);
        
        var arrived = (targetLocation.transform.position - targetAvatar.transform.position).magnitude < 0.5f;
        if (arrived) {
            targetAvatar.transform.position = targetLocation.transform.position;
        }

        var needMoreUpdate = !arrived; 
        return needMoreUpdate;
    }

}
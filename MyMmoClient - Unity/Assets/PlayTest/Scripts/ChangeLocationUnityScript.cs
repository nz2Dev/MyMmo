using System;
using System.Linq;
using MyMmo.Client;
using MyMmo.Commons.Scripts;
using Object = UnityEngine.Object;

public class ChangeLocationUnityScript : IUnityScript {

    private readonly Game game;
    private readonly ChangeLocationScriptData scriptData;

    private AvatarItem targetAvatar;
    private Location targetLocation;

    public ChangeLocationUnityScript(ChangeLocationScriptData scriptData) {
        this.scriptData = scriptData;
        
        targetAvatar = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(avatar => avatar.State.ItemId == scriptData.ItemId);
        if (targetAvatar == null) {
            throw new Exception("can't find script target item with id: " + scriptData.ItemId);
        }

        targetLocation = Object.FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == scriptData.ToLocation);
        if (targetLocation == null) {
            throw new Exception("can't find script's target location with id: " + scriptData.ToLocation);
        }
    }

    public void OnUpdateEnter() {
        targetAvatar.State.LocationId = scriptData.ToLocation;
        // todo Important! we could centralize state management, and use not Common.ItemSnapshotData
        // But client implementation of wrapper over that data, with all necessary api for state reset, move forward/backward etc. 
    }
    
    public void UpdateUnityState(float progress) {
        // no needs to call this for toggle changes, maybe use other interface for this changes updates
    }

    public void OnUpdateExit() {
        targetAvatar.State.LocationId = scriptData.ToLocation;
        // any of this methods should work, but maybe some animation or cleanups will take place here
    }

}
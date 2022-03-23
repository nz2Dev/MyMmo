using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using Object = UnityEngine.Object;

public class SpawnItemUnityScript : IUnityScript {

    private SpawnItemScriptData scriptData;

    private Location targetLocation;
    
    public SpawnItemUnityScript(SpawnItemScriptData scriptData) {
        this.scriptData = scriptData;

        targetLocation = Object.FindObjectsOfType<Location>()
            .FirstOrDefault(location => location.Id == scriptData.ItemSnapshotData.LocationId);
        if (targetLocation == null) {
            throw new Exception("can't find script's target location: " + scriptData.ItemSnapshotData.LocationId);
        }
    }

    public bool UpdateUnityState() {
        targetLocation.SpawnAvatar(PlayTest.Instance.playerPrefab, scriptData.ItemSnapshotData);
        return false;
    }

}
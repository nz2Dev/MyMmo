using System.Collections;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using UnityEngine;

public class Location : MonoBehaviour {

    public int Id;

    public void SpawnAvatar(GameObject playerPrefab, ItemSnapshotData snapshotData) {
        const int spawnHeight = 5;
        var centerOfLocation = transform.position;
        var initPosition = snapshotData.PositionInLocation.ToUnityVector3() + Vector3.up * spawnHeight;
        var player = Instantiate(playerPrefab, centerOfLocation + initPosition, Quaternion.identity);
        player.GetComponent<AvatarItem>().SetState(snapshotData);
    }

    public void ReplaceAvatar(GameObject playerPrefab, ItemSnapshotData itemSnapshotData) {
        var target = FindObjectsOfType<AvatarItem>().FirstOrDefault(i => i.State.ItemId == itemSnapshotData.ItemId);
        if (target != null) {
            Destroy(target.gameObject);
        }

        var centerOfLocation = transform.position;
        var initPosition = itemSnapshotData.PositionInLocation.ToUnityVector3();
        var player = Instantiate(playerPrefab, centerOfLocation + initPosition, Quaternion.identity);
        player.GetComponent<AvatarItem>().SetState(itemSnapshotData);
    }

    

}
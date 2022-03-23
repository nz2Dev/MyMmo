using System.Collections;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using UnityEngine;

public class Location : MonoBehaviour {

    public int Id;

    public void SpawnAvatar(GameObject playerPrefab, ItemSnapshotData snapshotData) {
        const int distanceOffset = 2;
        var centerOfLocation = transform.position;
        var avatarsOnLocation = FindObjectsOfType<AvatarItem>()
            .Where(avatar => avatar.state.LocationId == snapshotData.LocationId).ToArray();
        var radianStep = (1 / ((float) avatarsOnLocation.Length + 1) /*plus spawned*/) * Mathf.PI;
        for (var i = 0; i < avatarsOnLocation.Length; i++) {
            var direction = new Vector3(Mathf.Sin(radianStep * i), 0, Mathf.Cos(radianStep * i));
            avatarsOnLocation[i].MoveTo(centerOfLocation + direction * distanceOffset);
        }

        const int spawnHeight = 5;
        var spawnRadians = radianStep * avatarsOnLocation.Length + 1;
        var spawnDirectionFromCenter = new Vector3(Mathf.Sin(spawnRadians), spawnHeight, Mathf.Cos(spawnRadians));
        var player = Instantiate(playerPrefab, centerOfLocation + spawnDirectionFromCenter * distanceOffset,
            Quaternion.identity);
        player.GetComponent<AvatarItem>().state = snapshotData;
    }

    public void ReplaceAvatar(GameObject playerPrefab, ItemSnapshotData itemSnapshotData) {
        var target = FindObjectsOfType<AvatarItem>().FirstOrDefault(i => i.state.ItemId == itemSnapshotData.ItemId);
        if (target != null) {
            Destroy(target.gameObject);
        }

        var centerOfLocation = transform.position;
        var playerGO = Instantiate(playerPrefab,
            centerOfLocation + itemSnapshotData.PositionInLocation.ToUnityVector3(), Quaternion.identity);
        playerGO.GetComponent<AvatarItem>().SetState(itemSnapshotData);
    }

    public void ExecuteScripts(BaseScriptData[] scriptsData) {
        IEnumerator nextProcess = null;
        foreach (var script in scriptsData.Reverse()) {
            nextProcess = BuildCoroutine(UnityScriptFactory.Create(script), nextProcess);
        }

        StartCoroutine(nextProcess);
    }

    private IEnumerator BuildCoroutine(IUnityScript unityScript, IEnumerator continuation) {
        var needUpdate = true;
        while (needUpdate) {
            needUpdate = unityScript.UpdateUnityState();
            yield return new WaitForEndOfFrame();
        }

        if (continuation != null) {
            yield return StartCoroutine(continuation);
        }
    }

}
using System.Collections;
using System.Linq;
using MyMmo.Client;
using MyMmo.Commons.Scripts;
using UnityEngine;

public class Location : MonoBehaviour {

    public int Id;

    public void SpawnAvatar(GameObject playerPrefab, Item item) {
        const int distanceOffset = 2;
        var centerOfLocation = transform.position;
        var avatarsOnLocation = FindObjectsOfType<AvatarItem>().Where(avatar => avatar.source.LocationId == item.LocationId).ToArray();
        var radianStep = (1 / ((float) avatarsOnLocation.Length + 1) /*plus spawned*/) * Mathf.PI;
        for (var i = 0; i < avatarsOnLocation.Length; i++) {
            var direction = new Vector3(Mathf.Sin(radianStep * i), 0, Mathf.Cos(radianStep * i));
            avatarsOnLocation[i].MoveTo(centerOfLocation + direction * distanceOffset);
        }

        const int spawnHeight = 5;
        var spawnRadians = radianStep * avatarsOnLocation.Length + 1;
        var spawnDirectionFromCenter = new Vector3(Mathf.Sin(spawnRadians), spawnHeight, Mathf.Cos(spawnRadians));
        var player = Instantiate(playerPrefab, centerOfLocation + spawnDirectionFromCenter * distanceOffset, Quaternion.identity);
        player.GetComponent<AvatarItem>().source = item;
    }

    public void ExecuteScripts(ChangeLocationScript[] scripts, Game game) {
        IEnumerator nextProcess = null;
        foreach (var script in scripts.Reverse()) {
            nextProcess = BuildCoroutine(new ChangeLocationScriptExecutor(script, game), nextProcess);
        }
        StartCoroutine(nextProcess);
    }

    private IEnumerator BuildCoroutine(ChangeLocationScriptExecutor executor, IEnumerator continuation) {
        var needUpdate = true;
        while (needUpdate) {
            executor.Update();
            needUpdate = executor.IsRunning;
            yield return new WaitForEndOfFrame();
        }

        if (continuation != null) {
            yield return StartCoroutine(continuation);
        }
    }

}
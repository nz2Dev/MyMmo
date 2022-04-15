using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using UnityEngine;

namespace Player {
    public class UnityWorldPlayer : MonoBehaviour {

        // used to give global access of playerPrefab (e.g resource) to Spawning/Entering scripts,
        // todo make player prefab be references explicitly 
        public static UnityWorldPlayer Instance;
        
        public GameObject playerPrefab;

        private void Awake() {
            Instance = this;
        }

        public void EnterLocation(int locationId, SceneSnapshotData sceneSnapshotData) {
            var targetLocation = FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == locationId);
            if (targetLocation == null) {
                throw new Exception("Location not found: " + locationId);
            }
        
            Debug.Log($"location {locationId} enters, with entities snapshots [{sceneSnapshotData.EntitiesSnapshotData.AggregateToString()}]");
            foreach (var entitySnapshotData in sceneSnapshotData.EntitiesSnapshotData) {
                targetLocation.PlaceAvatar(playerPrefab, entitySnapshotData);
            }
        }

        public void ExitLocation(int locationId) {
            // for disposing unused resources, will have to implement some mechanism for that
            // or for camera work, etc. Modifying graphic representation of that location, applying fog of war, etc.
        }

        public void UpdateLocation(int locationId, ScriptsClipData scriptsClipData) {
            Debug.Log($"on location update: {locationId} with items[{scriptsClipData.ItemDataArray.Length}] [{scriptsClipData.ItemDataArray.Select(data => $"item {data.ItemId} scripts[" + data.ScriptDataArray.AggregateToString() + "]").AggregateToString()}]");
            var updatedLocation = FindObjectsOfType<Location>().First(location => location.Id == locationId);
            updatedLocation.SetClip(scriptsClipData);
        }
        
    }
}
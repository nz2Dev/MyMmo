using System;
using System.Collections.Generic;
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
        
        private readonly Dictionary<int, UnityScriptsPlayer> locationPlayers = new Dictionary<int, UnityScriptsPlayer>();

        private void Awake() {
            Instance = this;
        }

        public void EnterLocation(int locationId, SceneSnapshotData sceneSnapshotData) {
            var targetLocation = FindObjectsOfType<Location>().FirstOrDefault(location => location.id == locationId);
            if (targetLocation == null) {
                throw new Exception("Location not found: " + locationId);
            }
        
            Debug.Log($"location {locationId} enters, with entities snapshots [{sceneSnapshotData.EntitiesSnapshotData.AggregateToString()}]");
            foreach (var entitySnapshotData in sceneSnapshotData.EntitiesSnapshotData) {
                targetLocation.PlaceAvatar(playerPrefab, entitySnapshotData);
            }
            
            locationPlayers[locationId] = new UnityScriptsPlayer();
        }

        public void UpdateLocation(int locationId, ScriptsClipData scriptsClipData, Action onFinish = null) {
            Debug.Log($"on location update: {locationId} with items[{scriptsClipData.ItemDataArray.Length}] [{scriptsClipData.ItemDataArray.Select(data => $"item {data.ItemId} scripts[" + data.ScriptDataArray.AggregateToString() + "]").AggregateToString()}]");
            locationPlayers[locationId].SetClip(scriptsClipData, onFinish);
        }

        public void ExitLocation(int locationId) {
            Debug.Log($"location {locationId} exits from unity world");
            // for disposing unused resources, will have to implement some mechanism for that
            // or for camera work, etc. Modifying graphic representation of that location, applying fog of war, etc.
        }

        private void Update() {
            var locationsMap = FindObjectsOfType<Location>().ToDictionary(location => location.id);
            // making copy, so onFinish callback can modify location players,
            // this is workaround, but should be relatively cheap
            var thisFramePlayers = new Dictionary<int, UnityScriptsPlayer>(locationPlayers); 
            foreach (var playerEntry in thisFramePlayers) {
                playerEntry.Value.PlayNextFrame(locationsMap[playerEntry.Key]);
            }
        }

    }
}
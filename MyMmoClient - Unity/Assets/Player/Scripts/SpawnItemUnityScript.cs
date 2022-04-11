using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using ServerPlay;
using Object = UnityEngine.Object;

namespace Player.Scripts {
    public class SpawnItemUnityScript : IUnityScript {

        private SpawnItemScriptData scriptData;

        private Location targetLocation;
    
        public SpawnItemUnityScript(SpawnItemScriptData scriptData) {
            this.scriptData = scriptData;

            targetLocation = Object.FindObjectsOfType<Location>()
                .FirstOrDefault(location => location.Id == scriptData.EntitySnapshotData.LocationId);
            if (targetLocation == null) {
                throw new Exception("can't find script's target location: " + scriptData.EntitySnapshotData.LocationId);
            }
        }

        public void OnUpdateEnter() {
            targetLocation.SpawnAvatar(PlayTest.Instance.playerPrefab, scriptData.EntitySnapshotData);
        }

        public void UpdateUnityState(float progress) {
            // no needs for this
        }

        public void OnUpdateExit() {
            // one time command, can't spawn two times
        }

    }
}
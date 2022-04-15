using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using ServerPlay;
using Object = UnityEngine.Object;

namespace Player.Scripts {
    public class SpawnItemUnityScript : IUnityScript {

        private readonly SpawnItemScriptData scriptData;

        private Location targetLocation;
    
        public SpawnItemUnityScript(SpawnItemScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void OnUpdateEnter(int locationId) {
            targetLocation = Object.FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == locationId);
            if (targetLocation == null) {
                throw new Exception("can't find script's target location: " + locationId);
            }
            // todo use reference to world player instance instead
            targetLocation.SpawnAvatar(UnityWorldPlayer.Instance.playerPrefab, scriptData.EntitySnapshotData);
        }

        public void UpdateUnityState(int locationId, float progress) {
            // no needs for this
        }

        public void OnUpdateExit(int locationId) {
            // one time command, can't spawn two times
        }

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            // nothing yet.
        }

    }
}
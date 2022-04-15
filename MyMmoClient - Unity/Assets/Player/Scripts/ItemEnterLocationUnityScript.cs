using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using ServerPlay;
using Object = UnityEngine.Object;

namespace Player.Scripts {
    public class ItemEnterLocationUnityScript : IUnityScript {

        private readonly EnterItemScriptData scriptData;

        public ItemEnterLocationUnityScript(EnterItemScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void OnUpdateEnter(int locationId) {
            var targetLocation = Object.FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == locationId);
            if (targetLocation == null) {
                throw new Exception("can't find script's target location: " + locationId);
            }
            
            /*todo use reference to world player instance*/
            targetLocation.PlaceAvatar(UnityWorldPlayer.Instance.playerPrefab, scriptData.EntitySnapshotData);
        }

        public void UpdateUnityState(int locationId, float progress) {
        }

        public void OnUpdateExit(int locationId) {
        }

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            // nothing yet.
        }

    }
}
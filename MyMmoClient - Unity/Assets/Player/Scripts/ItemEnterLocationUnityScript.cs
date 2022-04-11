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

        public void OnUpdateEnter() {
            var targetLocation = Object.FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == scriptData.EntitySnapshotData.LocationId);
            if (targetLocation == null) {
                throw new Exception("can't find script's target location: " + scriptData.EntitySnapshotData.LocationId);
            }
            
            targetLocation.PlaceAvatar(PlayTest.Instance.playerPrefab, scriptData.EntitySnapshotData);
        }

        public void UpdateUnityState(float progress) {
        }

        public void OnUpdateExit() {
        }

    }
}
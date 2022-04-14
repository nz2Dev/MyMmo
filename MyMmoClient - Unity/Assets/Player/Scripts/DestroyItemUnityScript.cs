using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using Object = UnityEngine.Object;

namespace Player.Scripts {
    public class DestroyItemUnityScript : IUnityScript {

        private readonly DestroyItemScriptData scriptData;

        public DestroyItemUnityScript(DestroyItemScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void OnUpdateEnter(int locationId) {
            var targetItem = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(item => item.State.ItemId == scriptData.ItemId);
            if (targetItem == null) {
                throw new Exception($"target item {scriptData.ItemId} not found");
            }
            Object.Destroy(targetItem.gameObject);
        }

        public void UpdateUnityState(int locationId, float progress) {
            // no needs for this here
        }

        public void OnUpdateExit(int locationId) {
            // one time changes, can't delete two times
        }

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            // nothing yet.
        }

    }
}
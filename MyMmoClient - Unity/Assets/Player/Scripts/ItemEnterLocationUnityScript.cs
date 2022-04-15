using MyMmo.Commons.Scripts;

namespace Player.Scripts {
    public class ItemEnterLocationUnityScript : IUnityScript {

        private readonly EnterItemScriptData scriptData;

        public ItemEnterLocationUnityScript(EnterItemScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void OnUpdateEnter(Location location) {
            location.PlaceAvatar(UnityWorldPlayer.Instance.playerPrefab, scriptData.EntitySnapshotData);
        }

        public void UpdateUnityState(float progress) {
        }

        public void OnUpdateExit() {
        }

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            // nothing yet.
        }

    }
}
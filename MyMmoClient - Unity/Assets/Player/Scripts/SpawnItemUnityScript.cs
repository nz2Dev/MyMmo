using MyMmo.Commons.Scripts;

namespace Player.Scripts {
    public class SpawnItemUnityScript : IUnityScript {

        private readonly SpawnItemScriptData scriptData;

        public SpawnItemUnityScript(SpawnItemScriptData scriptData) {
            this.scriptData = scriptData;
        }

        public void OnUpdateEnter(Location location) {
            location.SpawnAvatar(UnityWorldPlayer.Instance.playerPrefab, scriptData.EntitySnapshotData);
        }

        public void UpdateUnityState(float progress) {
            // no needs for this
        }

        public void OnUpdateExit() {
            // one time command, can't spawn two times
        }

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            // nothing yet.
        }

    }
}
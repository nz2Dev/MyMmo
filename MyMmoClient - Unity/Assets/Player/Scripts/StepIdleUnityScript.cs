namespace Player.Scripts {
    public class StepIdleUnityScript : IUnityScript {
        
        // idles the whole step, so that other scripts in the list can be executed later
        // this is workaround initially for the ItemEnterLocationUnityScript
        
        public void OnUpdateEnter(int locationId) {
            // nothing.
        }

        public void UpdateUnityState(int locationId, float progress) {
            // nothing.
        }

        public void OnUpdateExit(int locationId) {
            // nothing.
        }

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            // nothing.
        }

    }
}
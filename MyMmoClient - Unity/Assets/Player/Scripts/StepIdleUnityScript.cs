namespace Player.Scripts {
    public class StepIdleUnityScript : IUnityScript {
        
        // idles the whole step, so that other scripts in the list can be executed later
        // this is workaround initially for the ItemEnterLocationUnityScript
        
        public void OnUpdateEnter(Location location) {
            // nothing.
        }

        public void UpdateUnityState(float progress) {
            // nothing.
        }

        public void OnUpdateExit() {
            // nothing.
        }

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            // nothing.
        }

    }
}
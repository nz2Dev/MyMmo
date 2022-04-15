namespace Player {
    public interface IUnityScript {

        void OnUpdateEnter(Location location);
    
        void UpdateUnityState(float progress);

        void OnUpdateExit();

        void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated);

    }
}
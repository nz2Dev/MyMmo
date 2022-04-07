namespace Player {
    public interface IUnityScript {

        void OnUpdateEnter();
    
        void UpdateUnityState(float progress);

        void OnUpdateExit();

    }
}
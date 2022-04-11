namespace Player {
    public interface IUnityScript {

        void OnUpdateEnter(int locationId);
    
        void UpdateUnityState(int locationId, float progress);

        void OnUpdateExit(int locationId);

    }
}
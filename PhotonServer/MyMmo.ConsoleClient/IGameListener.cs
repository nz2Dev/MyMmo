namespace MyMmo.ConsoleClient {
    public interface IGameListener {

        void OnConnected();
        
        void OnWorldCreated();

        void OnWorldEntered();

        void OnItemEnter(Item item);

        void OnItemExit(string itemId);

        void OnItemLocationChanged(Item item);

    }
}
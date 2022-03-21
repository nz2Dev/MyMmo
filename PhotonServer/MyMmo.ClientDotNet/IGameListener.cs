using ExitGames.Client.Photon;
using MyMmo.Commons.Scripts;

namespace MyMmo.Client {
    public interface IGameListener {

        void OnConnected();
        
        void OnWorldCreated();

        void OnWorldEntered();

        /// <summary>
        /// Item is in our interest area and now actively updates its state.
        /// It might be called multiple time on the same item, as well as after OnItemUnsubscribed.
        /// So the item state might differ between this calls.
        /// </summary>
        void OnItemSubscribed(Item item);

        /// <summary>
        /// Item is out of our interest area and now doesn't actively updates its state.
        /// However it might be subscribed again, so it's implementor responsibility to decide
        /// what to do with its representation of the item after unsubscribe.
        /// </summary>
        void OnItemUnsubscribed(Item item);

        /// <summary>
        /// Item is destroyed and should be gone.
        /// </summary>
        void OnItemDestroyed(Item item);

        void OnItemLocationChanged(Item item);

        void OnLog(DebugLevel debugLevel, string message);
        
        void OnRegionUpdate(int locationId, BaseScriptData[] scriptsData);

    }
}
using ExitGames.Client.Photon;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;

namespace MyMmo.Client {
    public interface IGameListener {

        void OnConnected();
        
        void OnWorldCreated();

        void OnWorldEntered();

        void OnLocationEntered(LocationSnapshotData locationSnapshotData);

        void OnLocationExit(int locationId);

        void OnLocationUpdate(int locationId, ScriptsClipData scriptsClipData);
        
        void OnLog(DebugLevel debugLevel, string message);

    }
}
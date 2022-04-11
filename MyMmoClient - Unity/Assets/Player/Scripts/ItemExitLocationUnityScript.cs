using System;
using System.Linq;
using MyMmo.Client;
using MyMmo.Commons.Scripts;
using Object = UnityEngine.Object;

namespace Player.Scripts {
    public class ItemExitLocationUnityScript : IUnityScript {

        private readonly Game game;
        private readonly ExitItemScriptData scriptData;

        private AvatarItem targetAvatar;
        private Location targetLocation;

        public ItemExitLocationUnityScript(ExitItemScriptData scriptData) {
            this.scriptData = scriptData;
        
            targetAvatar = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(avatar => avatar.State.ItemId == scriptData.ItemId);
            if (targetAvatar == null) {
                throw new Exception("can't find script target item with id: " + scriptData.ItemId);
            }
        }

        public void OnUpdateEnter(int locationId) {
            targetAvatar.DetachFromLocation();
            // todo Important! we could centralize state management, and use not Common.ItemSnapshotData
            // But client implementation of wrapper over that data, with all necessary api for state reset, move forward/backward etc. 
        }
    
        public void UpdateUnityState(int locationId, float progress) {
            // no needs to call this for toggle changes, maybe use other interface for this changes updates
        }

        public void OnUpdateExit(int locationId) {
            // any of this methods should work, but maybe some animation or cleanups will take place here
        }

    }
}
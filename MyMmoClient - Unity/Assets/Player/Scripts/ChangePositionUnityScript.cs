using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Player.Scripts {
    public class ChangePositionUnityScript : IUnityScript {

        private readonly ChangePositionScriptData scriptData;
        private Vector3 startPosition;
        private Vector3 targetPosition;
        private AvatarItem avatar;
        private Location avatarLocation;

        public ChangePositionUnityScript(ChangePositionScriptData scriptData) {
            this.scriptData = scriptData;
        
            avatar = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(avatar => avatar.State.ItemId == scriptData.ItemId);
            if (avatar == null) {
                throw new Exception("can't find script target item with id: " + scriptData.ItemId);
            }

            // todo fixme, it's not reliable, because we capture the references at construct time
            // second, is that location id is take from presentation, and is not guaranteed to be the case
            // the problem is that position from scriptData is in local space, and targetAvatar position that we trying to solve is in world space
            // this is just for now
            avatarLocation = Object.FindObjectsOfType<Location>().FirstOrDefault(location => location.Id == avatar.State.LocationId);
            if (avatarLocation == null) {
                throw new Exception("can't find script's target location: " + avatar.State.LocationId);
            }
        }

        public void OnUpdateEnter() {
            var locationCenter = avatarLocation.transform.position;
            startPosition = avatar.transform.position;
            targetPosition = locationCenter + scriptData.ToPosition.ToUnityVector3();
        }
    
        public void UpdateUnityState(float progress) {
            var newPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            avatar.Move(newPosition - avatar.transform.position);
        }

        public void OnUpdateExit() {
            avatar.transform.position = targetPosition;
            // for one time commands, they can do their staff in one of this methods
        }

    }
}
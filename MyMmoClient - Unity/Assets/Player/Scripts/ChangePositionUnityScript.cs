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

        public ChangePositionUnityScript(ChangePositionScriptData scriptData) {
            this.scriptData = scriptData;
        
            avatar = Object.FindObjectsOfType<AvatarItem>().FirstOrDefault(avatar => avatar.State.ItemId == scriptData.ItemId);
            if (avatar == null) {
                throw new Exception("can't find script target item with id: " + scriptData.ItemId);
            }
        }

        public void OnUpdateEnter(Location location) {
            var locationCenter = location.transform.position;
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

        public void OnUpdateDraw(UnityScriptsClipDrawer stateDrawer, bool activated) {
            stateDrawer.AddMovePoint(scriptData.ItemId, new Vector2(scriptData.ToPosition.X, scriptData.ToPosition.Y), activated);
        }

    }
}
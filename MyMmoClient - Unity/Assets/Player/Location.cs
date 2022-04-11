using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using Player.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player {
    
    [ExecuteInEditMode] 
    public class Location : MonoBehaviour {

        public int Id;
        public Transform AnnotationDrawingOffset;

        private UnityScriptsClipPlayer scriptsPlayer;

        private void Awake() {
            scriptsPlayer = GetComponent<UnityScriptsClipPlayer>();
            Assert.IsNotNull(scriptsPlayer);
        }

        public void SpawnAvatar(GameObject playerPrefab, EntitySnapshotData snapshotData) {
            const int spawnHeight = 5;
            var centerOfLocation = transform.position;
            var initPosition = snapshotData.PositionInLocation.ToUnityVector3() + Vector3.up * spawnHeight;
            var player = Instantiate(playerPrefab, centerOfLocation + initPosition, Quaternion.identity);
            player.GetComponent<AvatarItem>().AttachToLocation(Id, snapshotData);
            player.GetComponent<Rigidbody>().isKinematic = false;
        }

        public void PlaceAvatar(GameObject playerPrefab, EntitySnapshotData entitySnapshotData) {
            var target = FindObjectsOfType<AvatarItem>().FirstOrDefault(i => i.State.ItemId == entitySnapshotData.ItemId);
            if (target != null) {
                Destroy(target.gameObject);
            }

            var centerOfLocation = transform.position;
            var initPosition = entitySnapshotData.PositionInLocation.ToUnityVector3();
            var player = Instantiate(playerPrefab, centerOfLocation + initPosition, Quaternion.identity);
            player.GetComponent<AvatarItem>().AttachToLocation(Id, entitySnapshotData);
        }

        public void PlayClipImmediately(ScriptsClipData clipData, Action onFinish = null) {
            scriptsPlayer.PlayClip(Id, clipData, onFinish);
        }

        public void DrawShapesAnnotation() {
            scriptsPlayer.DrawShapesAnnotation(AnnotationDrawingOffset.localToWorldMatrix);
        }

    }
}
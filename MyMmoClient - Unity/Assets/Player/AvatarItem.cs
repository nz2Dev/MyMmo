using MyMmo.Commons.Snapshots;
using UnityEngine;

namespace Player {
    public class AvatarItem : MonoBehaviour {

        private Rigidbody capsuleRigidbody;
        private Vector3 displayVelocity = Vector3.zero;
    
        public ItemSnapshotData State { get; private set; }

        private void Awake() {
            capsuleRigidbody = GetComponentInChildren<Rigidbody>();
        }

        public void SetState(ItemSnapshotData snapshotData) {
            State = snapshotData;
        }

        private void OnCollisionEnter(Collision other) {
            capsuleRigidbody.isKinematic = true;
        }

        public void SetDisplayVelocity(Vector3 direction) {
            displayVelocity = direction;
        }

    }
}
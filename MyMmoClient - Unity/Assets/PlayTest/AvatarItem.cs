using MyMmo.Commons.Snapshots;
using UnityEngine;

public class AvatarItem : MonoBehaviour {

    private Rigidbody capsuleRigidbody;
    
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

}
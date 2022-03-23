using MyMmo.Commons.Snapshots;
using UnityEngine;

public class AvatarItem : MonoBehaviour {

    public ItemSnapshotData state;

    private Rigidbody capsuleRigidbody;
    
    private void Awake() {
        capsuleRigidbody = GetComponentInChildren<Rigidbody>();
    }

    public void SetState(ItemSnapshotData snapshotData) {
        state = snapshotData;
    }

    public void MoveTo(Vector3 position) {
        transform.position = position;
    }

    private void OnCollisionEnter(Collision other) {
        capsuleRigidbody.isKinematic = true;
    }

}
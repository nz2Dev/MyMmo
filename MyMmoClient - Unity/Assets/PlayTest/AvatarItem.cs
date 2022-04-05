using MyMmo.Commons.Snapshots;
using UnityEngine;

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

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + displayVelocity.normalized);
    }

}
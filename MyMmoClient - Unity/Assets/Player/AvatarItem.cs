using MyMmo.Commons.Snapshots;
using Shapes;
using UnityEngine;

namespace Player {
    
    public class AvatarItem : MonoBehaviour {

        private Rigidbody capsuleRigidbody;
        private Vector3 heading = Vector3.zero;
    
        public EntitySnapshotData State { get; private set; } = new EntitySnapshotData();
        public int LocationId { get; private set; }
        public bool TransitiveState { get; private set; }

        private void Awake() {
            capsuleRigidbody = GetComponentInChildren<Rigidbody>();
        }

        public void AttachToLocation(int locationId, EntitySnapshotData snapshotData) {
            LocationId = locationId;
            State = snapshotData;
        }
        
        public void DetachFromLocation() {
            TransitiveState = true;
        }

        public void Move(Vector3 translation) {
            transform.Translate(translation, Space.World);
            heading = translation;
        }

        private void Update() {
            if (heading.magnitude > 0) {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(heading.normalized, Vector3.up), Time.deltaTime * 5);
            }
        }

        private void OnCollisionEnter(Collision other) {
            capsuleRigidbody.isKinematic = true;
            var pos = transform.position; 
            transform.position = new Vector3(pos.x, 0, pos.z);
        }

        public void DrawShapesAnnotation() {
            Draw.ResetAllDrawStates();
            Draw.ZOffsetFactor = -1;
            Draw.ZOffsetUnits = -10;            
            Draw.Matrix = transform.localToWorldMatrix;
            Draw.Rotate(Mathf.PI / 2, Vector3.right);

            Draw.UseDashes = true;
            Draw.LineGeometry = LineGeometry.Flat2D;
            Draw.ThicknessSpace = ThicknessSpace.Meters;
            Draw.DashStyle = DashStyle.FixedDashCount(DashType.Basic, 4, snap: DashSnapping.EndToEnd);
            Draw.Line(Vector3.zero, Vector3.up * 2f, 0.25f, LineEndCap.None, Color.clear, Color.white);
            Draw.Cone(Vector3.up * 2f, Vector3.up, 0.2f, 0.2f, Color.white);
        }
    }
}
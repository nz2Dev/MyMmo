using Shapes;
using UnityEngine;

namespace Player {
    public class UnityScriptsPlayerDrawer : AnnotationShapes {

        public bool drawInScene;
        public float clipStartTime;
        public float clipDuration = 5;
        public float globalTime = 1;

        public override void OnDrawAnnotations() {
            Draw.ResetAllDrawStates();
            Draw.Matrix = transform.localToWorldMatrix;
            Draw.ThicknessSpace = ThicknessSpace.Meters;
            Draw.Rotate(Quaternion.LookRotation(Vector3.down, Vector3.forward));
            
            if (Application.isPlaying || drawInScene) {
                DrawSlidingClip(globalTime, clipStartTime, clipDuration);
            }
        }

        private static void DrawSlidingClip(float time, float startTime, float duration) {
            const float height = 0.4f;

            var timeSliding = time - startTime;
            var durationProgress = Mathf.Clamp(timeSliding, 0, duration);
                
            Draw.RectangleBorder(Vector3.right * -timeSliding, duration, height, RectPivot.Corner, 0.1f, Color.green);
            Draw.Rectangle(Vector3.right * -timeSliding, durationProgress, height, RectPivot.Corner, Color.green);
        }

    }
}
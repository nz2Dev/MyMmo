using Shapes;
using UnityEngine;

namespace Player {
    public class UnityScriptsPlayerDrawer : AnnotationShapes {

        private const float CLIP_SHAPE_HEIGHT = 0.4f;

        public bool drawInScene;
        public float clipStartTime;
        public float clipDuration = 5;
        public float globalTime = 1;

        public override void OnDrawAnnotations() {
            if (Application.isPlaying || drawInScene) {
                Draw.ResetAllDrawStates();
                Draw.Matrix = transform.localToWorldMatrix;
                Draw.ThicknessSpace = ThicknessSpace.Meters;
                Draw.Rotate(Quaternion.LookRotation(Vector3.down, Vector3.forward));

                DrawSlidingClip(globalTime, clipStartTime, clipDuration);
                DrawTimeCursor();
            }
        }

        private static void DrawTimeCursor() {
            const float halfCursorHeight = CLIP_SHAPE_HEIGHT;
            Draw.Line(Vector3.down * halfCursorHeight, Vector3.up * halfCursorHeight, 0.125f, Color.blue);
        }

        private static void DrawSlidingClip(float time, float startTime, float duration) {
            var timeSliding = time - startTime;
            var durationProgress = Mathf.Clamp(timeSliding, 0, duration);
            
            var verticalOffset = new Vector3(0, -CLIP_SHAPE_HEIGHT / 2f);
            var position = Vector3.right * -timeSliding + verticalOffset;
            Draw.RectangleBorder(position, duration, CLIP_SHAPE_HEIGHT, RectPivot.Corner, 0.1f, Color.green);
            Draw.Rectangle(position, durationProgress, CLIP_SHAPE_HEIGHT, RectPivot.Corner, Color.green);
        }

    }
}
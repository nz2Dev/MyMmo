using System;
using MyMmo.Commons.Scripts;
using Shapes;
using UnityEngine;

namespace Player {
    public class UnityScriptsPlayer : MonoBehaviour {

        public bool debugAnnotationDrawing;
        public float playerTimeDebug = 2;

        private UnityScriptsClip unityScriptsClip;
        private float timePassed;
        private Action onFinish;

        public void SetClip(ScriptsClipData clip, Action onFinishPlaying = null) {
            unityScriptsClip = new UnityScriptsClip(clip);
            onFinish = onFinishPlaying;
            timePassed = 0;
        }

        public void DrawShapesAnnotation(Matrix4x4 worldTransformationMatrix) {
            const float height = 0.4f;
            
            Draw.ResetAllDrawStates();
            Draw.Matrix = worldTransformationMatrix;
            Draw.ThicknessSpace = ThicknessSpace.Meters;
            Draw.Rotate(Quaternion.LookRotation(Vector3.down, Vector3.forward));
            
            void DrawClipProgress(float time, float startTime, float duration) {
                var timeOnClip = time - startTime;
                var clipTimeProgress = Mathf.Clamp(timeOnClip, 0, duration);
                
                Draw.RectangleBorder(Vector3.right * -timeOnClip, duration, height, RectPivot.Corner, 0.1f, Color.green);
                Draw.Rectangle(Vector3.right * -timeOnClip, clipTimeProgress, height, RectPivot.Corner, Color.green);
            }
            
            if (!Application.isPlaying && debugAnnotationDrawing) {
                DrawClipProgress(playerTimeDebug, 1, 5);
            } else if (unityScriptsClip != null) {
                DrawClipProgress(timePassed, 0, unityScriptsClip.Length());
            }
        }

        public void PlayNextFrame(int locationId) {
            if (unityScriptsClip == null) {
                return;
            }

            if (unityScriptsClip.Length() < timePassed) {
                return;
            }
            
            timePassed += Time.deltaTime;
            unityScriptsClip.SampleState(locationId, timePassed);
            
            if (unityScriptsClip.Length() < timePassed) {
                onFinish?.Invoke();
            }
        }
    }
}
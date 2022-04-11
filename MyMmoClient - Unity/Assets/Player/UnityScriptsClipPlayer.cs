using System;
using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using Player.Scripts;
using Shapes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player {
    public class UnityScriptsClipPlayer : MonoBehaviour {

        public bool debugAnnotationDrawing;
            
        private List<Track> scriptTracks = new List<Track>();
        private float timePassed;
        private Action onFinish;

        public void PlayClip(ScriptsClipData clip, Action onFinishPlaying = null) {
            onFinish = onFinishPlaying;

            var firstItemId = clip.ItemDataArray.ElementAtOrDefault(0)?.ItemId;
            
            timePassed = 0;
            scriptTracks = clip.ItemDataArray.Select(data => new Track(
                data.ScriptDataArray.Select(UnityScriptFactory.Create).ToList(),
                clip.ChangesDeltaTime,
                data.ItemId == firstItemId
            )).ToList();
        }

        public void DrawShapesAnnotation(Matrix4x4 worldTransformationMatrix) {
            Draw.ResetAllDrawStates();
            Draw.Matrix = worldTransformationMatrix;
            Draw.ThicknessSpace = ThicknessSpace.Meters;

            const float maxWidth = 5;
            const float maxHeight = 0.4f;
            Draw.Translate(-maxWidth / 2f, 0, -maxHeight / 2f);
            Draw.Rotate(Quaternion.LookRotation(Vector3.down, Vector3.forward));

            void DrawTrackProgress(Vector3 position, float progress) {
                Draw.Rectangle(position, maxWidth, maxHeight, RectPivot.Corner, Color.white);
                Draw.Rectangle(position, maxWidth * progress, maxHeight, RectPivot.Corner, Color.green);    
            }

            if (!Application.isPlaying && debugAnnotationDrawing) {
                DrawTrackProgress(Vector3.zero, 0.5f); // for debug
            }
            for (var i = 0; i < scriptTracks.Count; i++) {
                DrawTrackProgress((maxHeight + 0.1f) * i * Vector3.down, scriptTracks[i].GetTimeProgress(timePassed));
            }
        }

        private void Update() {
            if (scriptTracks.Count == 0) {
                return;
            }
            
            timePassed += Time.deltaTime;
            scriptTracks.RemoveAll(track => !track.Play(timePassed));
            if (scriptTracks.Count == 0) {
                onFinish?.Invoke();                
            }
        }

        class Track {

            private readonly float segmentTimeLength;
            private readonly List<IUnityScript> unityScripts;
            private float currentSegmentEnterTime;
            private int currentSegmentIndex = -1;
            
            private float timePassedDebug;
            private float frameTimeDeltaDebug;
            private bool debug;

            public Track(List<IUnityScript> unityScripts, float segmentTimeLength, bool debug) {
                this.unityScripts = unityScripts;
                this.segmentTimeLength = segmentTimeLength;
                this.debug = debug;
            }

            public float GetTimeProgress(float timePassed) {
                return timePassed / (unityScripts.Count * segmentTimeLength);
            }
            
            public bool Play(float timePassed) {
                if (currentSegmentIndex == -1) {
                    if (!EnterNextSegment(timePassed)) {
                        return false;
                    }
                }

                var timeDelta = timePassed - currentSegmentEnterTime;
                timePassedDebug = timePassed;
                frameTimeDeltaDebug = Time.deltaTime;
                if (timeDelta > segmentTimeLength) {
                    ExitCurrentSegment();
                    // ...should return next segment, then enter/exit all segments in between,
                    // so that state is correct, and all scripts contributes,
                    // in case if each next script don't have previous state... 
                    if (!EnterNextSegment(timePassed)) {
                        return false;
                    }

                    var newTimeDelta = timePassed - currentSegmentEnterTime;
                    Assert.IsTrue(newTimeDelta > 0 && newTimeDelta < segmentTimeLength);
                    CurrentSegmentInterpolation(newTimeDelta / segmentTimeLength);
                    return true;
                }

                if (timeDelta < segmentTimeLength) {
                    CurrentSegmentInterpolation(timeDelta / segmentTimeLength);
                    return true;
                }

                if (timeDelta < 0) {
                    throw new Exception("Can't play backwards for now");
                }

                throw new Exception("Unexpected play exit");
            }

            private void ExitCurrentSegment() {
                var currSegment = unityScripts.ElementAtOrDefault(currentSegmentIndex);
                if (currSegment != null) {
                    currSegment.UpdateUnityState(progress: 1);
                    currSegment.OnUpdateExit();
                    if (debug) {
                        Debug.Log($"[{currentSegmentIndex}] |{currentSegmentEnterTime}s .. {frameTimeDeltaDebug}/{timePassedDebug}s >> ");
                    }
                }
            }

            private bool EnterNextSegment(float timePassed) {
                var segmentsPassedApprox = timePassed / segmentTimeLength;
                var segmentsPassedCount = Mathf.FloorToInt(segmentsPassedApprox);

                var nextSegmentIndex = segmentsPassedCount;
                var nextScript = unityScripts.ElementAtOrDefault(nextSegmentIndex);
                if (nextScript != null) {
                    currentSegmentIndex = nextSegmentIndex;
                    currentSegmentEnterTime = segmentsPassedCount * segmentTimeLength;
                    nextScript.OnUpdateEnter();
                    if (debug) {
                        Debug.Log($"<< .. {frameTimeDeltaDebug}/{timePassedDebug}s .. {currentSegmentEnterTime}s| [{currentSegmentIndex}]");
                    }
                    return true;
                } else {
                    return false;
                }
            }

            private void CurrentSegmentInterpolation(float progress) {
                unityScripts[currentSegmentIndex].UpdateUnityState(progress);
                if (debug) {
                    Debug.Log($"[{currentSegmentIndex}] |{currentSegmentEnterTime}s .. {frameTimeDeltaDebug}/{timePassedDebug}s .. ");
                }
            }

        }

    }
}
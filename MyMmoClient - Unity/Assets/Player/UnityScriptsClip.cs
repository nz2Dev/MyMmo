using System;
using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using Player.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player {
    public class UnityScriptsClip {

        private readonly List<Track> scriptTracks;

        public UnityScriptsClip(ScriptsClipData clip) {
            scriptTracks = clip.ItemDataArray.Select(data => new Track(
                data.ScriptDataArray.Select(UnityScriptFactory.Create).ToList(),
                clip.ChangesDeltaTime,
                false
            )).ToList();
        }

        public float Length() {
            return scriptTracks.Select(track => track.Length()).Max();
        }

        public void DrawState(UnityScriptsClipDrawer stateDrawer, float timePassed) {
            foreach (var scriptTrack in scriptTracks) {
                scriptTrack.DrawState(stateDrawer, timePassed);
            }
        }
        
        public void SampleState(int locationId, float timePassed) {
            foreach (var scriptTrack in scriptTracks) {
                if (!scriptTrack.IsEndOfState) {
                    scriptTrack.SampleState(locationId, timePassed);
                }
            }
        }
        
        public class Track {

            private readonly float segmentTimeLength;
            private readonly List<IUnityScript> unityScripts;
            private float currentSegmentEnterTime;
            private int currentSegmentIndex = -1;
            
            private float timePassedDebug;
            private float frameTimeDeltaDebug;
            private bool endOfState;
            private bool debug;

            public Track(List<IUnityScript> unityScripts, float segmentTimeLength, bool debug) {
                this.unityScripts = unityScripts;
                this.segmentTimeLength = segmentTimeLength;
                this.debug = debug;
            }

            public bool IsEndOfState => endOfState; 
            
            public float Length() {
                return unityScripts.Count * segmentTimeLength;
            }

            public void DrawState(UnityScriptsClipDrawer stateDrawer, float timePassed) {
                for (var index = 0; index < unityScripts.Count; index++) {
                    var unityScript = unityScripts[index];
                    var scriptEndTime = (index + 1) * segmentTimeLength;
                    unityScript.OnUpdateDraw(stateDrawer, scriptEndTime < timePassed);
                }
            }
            
            public void SampleState(int locationId, float timePassed) {
                if (endOfState) {
                    return;
                }
                
                if (currentSegmentIndex == -1) {
                    if (!EnterNextSegment(locationId, timePassed)) {
                        endOfState = true;
                        return;
                    }
                }

                var timeDelta = timePassed - currentSegmentEnterTime;
                timePassedDebug = timePassed;
                frameTimeDeltaDebug = Time.deltaTime;
                if (timeDelta > segmentTimeLength) {
                    ExitCurrentSegment(locationId);
                    // ...should return next segment, then enter/exit all segments in between,
                    // so that state is correct, and all scripts contributes,
                    // in case if each next script don't have previous state... 
                    if (!EnterNextSegment(locationId, timePassed)) {
                        endOfState = true; // workaround to prevent inconsistent behaviour when sampling out of range multiple time 
                        return;
                    }

                    var newTimeDelta = timePassed - currentSegmentEnterTime;
                    Assert.IsTrue(newTimeDelta > 0 && newTimeDelta < segmentTimeLength);
                    CurrentSegmentInterpolation(locationId, newTimeDelta / segmentTimeLength);
                    return;
                }

                if (timeDelta < segmentTimeLength) {
                    CurrentSegmentInterpolation(locationId, timeDelta / segmentTimeLength);
                    return;
                }

                if (timeDelta < 0) {
                    throw new Exception("Can't sample backwards for now");
                }

                throw new Exception("Unexpected sample exit");
            }

            private void ExitCurrentSegment(int locationId) {
                var currSegment = unityScripts.ElementAtOrDefault(currentSegmentIndex);
                if (currSegment != null) {
                    currSegment.UpdateUnityState(locationId, progress: 1);
                    currSegment.OnUpdateExit(locationId);
                    if (debug) {
                        Debug.Log($"[{currentSegmentIndex}] |{currentSegmentEnterTime}s .. {frameTimeDeltaDebug}/{timePassedDebug}s >> ");
                    }
                }
            }

            private bool EnterNextSegment(int locationId, float timePassed) {
                var segmentsPassedApprox = timePassed / segmentTimeLength;
                var segmentsPassedCount = Mathf.FloorToInt(segmentsPassedApprox);

                var nextSegmentIndex = segmentsPassedCount;
                var nextScript = unityScripts.ElementAtOrDefault(nextSegmentIndex);
                if (nextScript != null) {
                    currentSegmentIndex = nextSegmentIndex;
                    currentSegmentEnterTime = segmentsPassedCount * segmentTimeLength;
                    nextScript.OnUpdateEnter(locationId);
                    if (debug) {
                        Debug.Log($"<< .. {frameTimeDeltaDebug}/{timePassedDebug}s .. {currentSegmentEnterTime}s| [{currentSegmentIndex}]");
                    }
                    return true;
                } else {
                    return false;
                }
            }

            private void CurrentSegmentInterpolation(int locationId, float progress) {
                unityScripts[currentSegmentIndex].UpdateUnityState(locationId, progress);
                if (debug) {
                    Debug.Log($"[{currentSegmentIndex}] |{currentSegmentEnterTime}s .. {frameTimeDeltaDebug}/{timePassedDebug}s .. ");
                }
            }

        }
    }
}
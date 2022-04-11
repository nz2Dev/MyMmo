using System;
using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using Player.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player {
    public class UnityScriptsClipPlayer : MonoBehaviour {

        private List<Track> scriptTracks;
        private float timePassed;
        private Action onFinish;

        public void PlayClip(int locationId, ScriptsClipData clip, Action onFinishPlaying = null) {
            onFinish = onFinishPlaying;

            timePassed = 0;
            scriptTracks = clip.ItemDataArray.Select(data => new Track(
                data.ScriptDataArray.Select(UnityScriptFactory.Create).ToList(),
                clip.ChangesDeltaTime
            )).ToList();
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
            private int currentSegmentIndex;

            public Track(List<IUnityScript> unityScripts, float segmentTimeLength) {
                this.unityScripts = unityScripts;
                this.segmentTimeLength = segmentTimeLength;
            }

            public bool Play(float timePassed) {
                if (currentSegmentIndex == -1) {
                    if (!EnterNextSegment(timePassed)) {
                        return false;
                    }
                }

                var timeDelta = timePassed - currentSegmentEnterTime;
                if (timeDelta > segmentTimeLength) {
                    ExitCurrentSegment();
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
                }
            }

            private bool EnterNextSegment(float timePassed) {
                var segmentsPassedApprox = timePassed / segmentTimeLength;
                var segmentsPassedCount = Mathf.FloorToInt(segmentsPassedApprox);

                var nextSegmentIndex = segmentsPassedCount - 1;
                var nextScript = unityScripts.ElementAtOrDefault(nextSegmentIndex);
                if (nextScript != null) {
                    currentSegmentIndex = nextSegmentIndex;
                    currentSegmentEnterTime = segmentsPassedCount * segmentTimeLength;
                    nextScript.OnUpdateEnter();
                    return true;
                } else {
                    return false;
                }
            }

            private void CurrentSegmentInterpolation(float progress) {
                unityScripts[currentSegmentIndex].UpdateUnityState(progress);
            }

        }

    }
}
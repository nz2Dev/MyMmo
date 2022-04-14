using System;
using MyMmo.Commons.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player {
    public class UnityScriptsPlayer : MonoBehaviour {

        public UnityScriptsPlayerDrawer drawer;
        
        private UnityScriptsClip singleClip;
        private float timePassed;
        private Action onFinish;

        private void Awake() {
            Assert.IsNotNull(drawer);
        }

        public void SetClip(ScriptsClipData clip, Action onFinishPlaying = null) {
            singleClip = new UnityScriptsClip(clip);
            onFinish = onFinishPlaying;
            timePassed = 0;

            drawer.clipDuration = singleClip.Length();
            drawer.clipStartTime = 0f; // for now
            drawer.globalTime = timePassed;
        }

        public void PlayNextFrame(int locationId) {
            if (singleClip == null) {
                return;
            }

            if (singleClip.Length() < timePassed) {
                return;
            }
            
            timePassed += Time.deltaTime;
            singleClip.SampleState(locationId, timePassed);
            drawer.globalTime = timePassed;
            
            if (singleClip.Length() < timePassed) {
                onFinish?.Invoke();
            }
        }
    }
}
using System;
using MyMmo.Commons.Scripts;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player {
    public class UnityScriptsPlayer : MonoBehaviour {

        public UnityScriptsPlayerDrawer playerDrawer;
        public UnityScriptsClipDrawer clipDrawer;
        
        private UnityScriptsClip singleClip;
        private float timePassed;
        private Action onFinish;

        private void Awake() {
            Assert.IsNotNull(playerDrawer);
            Assert.IsNotNull(clipDrawer);
        }

        public void SetClip(ScriptsClipData clip, Action onFinishPlaying = null) {
            singleClip = new UnityScriptsClip(clip);
            onFinish = onFinishPlaying;
            timePassed = 0;

            playerDrawer.clipDuration = singleClip.Length();
            playerDrawer.clipStartTime = 0f; // for now
            playerDrawer.globalTime = timePassed;
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
            playerDrawer.globalTime = timePassed;
            
            clipDrawer.Clear();
            singleClip.DrawState(clipDrawer, timePassed);

            if (singleClip.Length() < timePassed) {
                onFinish?.Invoke();
            }
        }
    }
}
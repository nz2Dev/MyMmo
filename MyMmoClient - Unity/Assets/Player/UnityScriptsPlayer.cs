using System;
using MyMmo.Commons.Scripts;
using UnityEngine;

namespace Player {
    public class UnityScriptsPlayer {
        
        private UnityScriptsClip singleClip;
        private float timePassed;
        private Action onFinish;
        
        public void SetClip(ScriptsClipData clip, Action onFinishPlaying = null) {
            singleClip = new UnityScriptsClip(clip);
            onFinish = onFinishPlaying;
            timePassed = 0;
        }

        public void PlayNextFrame(Location location) {
            if (singleClip == null) {
                return;
            }

            if (singleClip.Length() < timePassed) {
                return;
            }
            
            timePassed += Time.deltaTime;
            singleClip.SampleState(location, timePassed);
            location.clipPlayerDrawer.globalTime = timePassed;
            location.clipPlayerDrawer.clipDuration = singleClip.Length();
            location.clipPlayerDrawer.clipStartTime = 0f; // for now

            location.clipsDrawer.Clear();
            singleClip.DrawState(location.clipsDrawer, timePassed);

            if (singleClip.Length() < timePassed) {
                onFinish?.Invoke();
            }
        }
    }
}
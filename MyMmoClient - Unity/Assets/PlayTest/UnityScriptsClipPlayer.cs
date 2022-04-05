using System;
using System.Collections;
using System.Linq;
using MyMmo.Commons.Scripts;
using UnityEngine;

public class UnityScriptsClipPlayer : MonoBehaviour {

    public void PlayClip(int locationId, ScriptsClipData clip, Action onFinishPlaying = null) {
        var countdown = clip.ItemDataArray.Length;

        void CountdownAction() {
            countdown--;
            if (countdown == 0) {
                onFinishPlaying?.Invoke();
            }
        }

        foreach (var itemData in clip.ItemDataArray) {
            StartScripts(clip.ChangesDeltaTime, itemData.ScriptDataArray, BuildFinisher(CountdownAction));
        }
    }

    private IEnumerator BuildFinisher(Action countdown) {
        yield return new WaitForSeconds(1);
        countdown();
    }

    public void StartScripts(float changesDeltaTime, BaseScriptData[] scriptsData, IEnumerator onFinish) {
        IEnumerator nextProcess = onFinish;
        foreach (var script in scriptsData.Reverse()) {
            nextProcess = BuildCoroutine(changesDeltaTime, UnityScriptFactory.Create(script), nextProcess);
        }

        StartCoroutine(nextProcess);
    }

    private IEnumerator BuildCoroutine(float changesDeltaTime, IUnityScript unityScript, IEnumerator continuation) {
        var timePassed = 0f;
        
        unityScript.OnUpdateEnter();
        while (timePassed < changesDeltaTime) {
            timePassed += Time.deltaTime;
            var progress = timePassed / changesDeltaTime;
            unityScript.UpdateUnityState(progress);
            yield return new WaitForEndOfFrame();
        }

        unityScript.OnUpdateExit();
        if (continuation != null) {
            yield return StartCoroutine(continuation);
        }
    }

}
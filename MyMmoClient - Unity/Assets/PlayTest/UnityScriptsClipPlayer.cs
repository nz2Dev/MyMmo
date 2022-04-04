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
            StartScripts(itemData.ScriptDataArray, BuildFinisher(CountdownAction));
        }
    }

    private IEnumerator BuildFinisher(Action countdown) {
        yield return new WaitForSeconds(1);
        countdown();
    }

    public void StartScripts(BaseScriptData[] scriptsData, IEnumerator onFinish) {
        IEnumerator nextProcess = onFinish;
        foreach (var script in scriptsData.Reverse()) {
            nextProcess = BuildCoroutine(UnityScriptFactory.Create(script), nextProcess);
        }

        StartCoroutine(nextProcess);
    }

    private IEnumerator BuildCoroutine(IUnityScript unityScript, IEnumerator continuation) {
        var needUpdate = true;
        var timePassed = 0f;
        while (needUpdate) {
            timePassed += Time.deltaTime;
            needUpdate = unityScript.UpdateUnityState(timePassed);
            yield return new WaitForEndOfFrame();
        }

        if (continuation != null) {
            yield return StartCoroutine(continuation);
        }
    }

}
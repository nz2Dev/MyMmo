using System.Collections;
using System.Linq;
using MyMmo.Commons.Scripts;
using UnityEngine;

public class UnityScriptsClipPlayer : MonoBehaviour {

    public void PlayClip(int locationId, ScriptsClipData clip) {
        foreach (var itemData in clip.ItemDataArray) {
            StartScripts(itemData.ScriptDataArray);
        }
    }

    public void StartScripts(BaseScriptData[] scriptsData) {
        IEnumerator nextProcess = null;
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
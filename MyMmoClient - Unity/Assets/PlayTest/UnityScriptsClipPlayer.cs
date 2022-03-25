using System.Collections;
using System.Linq;
using MyMmo.Commons.Scripts;
using UnityEngine;

public class UnityScriptsClipPlayer : MonoBehaviour {

    public void PlayClip(int locationId, ScriptsClipData data) {
        foreach (var itemScriptsData in data.ScriptsData) {
            StartScripts(new[] {itemScriptsData.ItemScriptData});
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
        while (needUpdate) {
            needUpdate = unityScript.UpdateUnityState();
            yield return new WaitForEndOfFrame();
        }

        if (continuation != null) {
            yield return StartCoroutine(continuation);
        }
    }

}
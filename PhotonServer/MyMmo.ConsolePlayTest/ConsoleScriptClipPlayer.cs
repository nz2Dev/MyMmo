using System.Collections.Generic;
using MyMmo.Commons.Scripts;
using MyMmo.ConsolePlayTest.Scripts;

namespace MyMmo.ConsolePlayTest {
    public class ConsoleScriptClipPlayer {
        
        public static void Play(int locationId, ScriptsClipData clipData, Dictionary<string, ConsoleItem> state) {
            foreach (var itemScriptsData in clipData.ItemDataArray) {
                foreach (var scriptData in itemScriptsData.ScriptDataArray) {
                    var applier = ConsoleScriptApplierFactory.Create(scriptData);
                    applier.ApplyClientState(locationId, state);    
                }
            }
        }

    }
}
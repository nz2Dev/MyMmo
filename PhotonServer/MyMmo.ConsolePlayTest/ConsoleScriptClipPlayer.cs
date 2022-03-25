using System.Collections.Generic;
using MyMmo.Commons.Scripts;
using MyMmo.ConsolePlayTest.Scripts;

namespace MyMmo.ConsolePlayTest {
    public class ConsoleScriptClipPlayer {
        
        public static void Play(ScriptsClipData clipData, Dictionary<string, ConsoleItem> state) {
            foreach (var itemScriptsData in clipData.ScriptsData) {
                var applier = ConsoleScriptApplierFactory.Create(itemScriptsData.ItemScriptData);
                applier.ApplyClientState(state);
            }
        }

    }
}
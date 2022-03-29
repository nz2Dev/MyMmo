using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Game {
    public class LocationScriptsClip {
        
        private Dictionary<string, IScript> scripts = new Dictionary<string, IScript>();
        
        public void SetItemScript(string itemId, IScript script) {
            scripts[itemId] = script;
        }

        // api changes draft
        
        public void SetItemScriptIntent(string itemId, IScript script) {
            scripts[itemId] = script;
        }

        public IScript GetItemLastScriptIntent(string itemId) {
            return scripts[itemId];
        }
        
        public IEnumerable<IScript> LastIntents() {
            return scripts.Values;
        }
        
        // end draft

        public void ApplyState(World world) {
            foreach (var script in scripts.Values) {
                script.ApplyState(world);
            }
        }

        public ScriptsClipData ToData() {
            var itemsData = scripts.Select(pair => new ItemScriptsData {
                ItemId = pair.Key,
                ItemScriptData = pair.Value.ToScriptData()
            });

            return new ScriptsClipData {
                ScriptsData = itemsData.ToArray()
            };
        }

    }
}
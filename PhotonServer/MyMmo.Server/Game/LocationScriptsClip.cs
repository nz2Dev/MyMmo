using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Game {
    public class LocationScriptsClip {

        private Dictionary<string, List<IScript>> scripts = new Dictionary<string, List<IScript>>();

        public void AddItemScript(string itemId, IScript script) {
            GetOrCreateScriptsListForId(itemId).Add(script);
        }

        public bool TryGetLastItemScriptOf<T>(string itemId, out T lastScript) where T : IScript {
            var itemScripts = GetOrCreateScriptsListForId(itemId);
            if (itemScripts.Count == 0) {
                lastScript = default;
                return false;
            } else {
                var lastScriptObject = itemScripts[itemScripts.Count - 1];
                if (lastScriptObject is T script) {
                    lastScript = script;
                    return true;
                } else {
                    lastScript = default;
                    return false;
                }
            }
        }

        public void ApplyState(World world) {
            var queues = scripts.Values.Select(list => new Queue<IScript>(list)).ToList();
            while (queues.Count > 0) {
                foreach (var scriptQueue in queues) {
                    scriptQueue.Dequeue().ApplyState(world);
                }

                queues.RemoveAll(queue => queue.Count == 0);
            }
        }

        public ScriptsClipData ToData() {
            var itemData = scripts.Select(clipRecord => {
                var scriptsData = clipRecord.Value.Select(script => script.ToScriptData());
                return new ItemScriptsData {
                    ItemId = clipRecord.Key,
                    ScriptDataArray = scriptsData.ToArray()
                };
            });

            return new ScriptsClipData {
                ItemDataArray = itemData.ToArray()
            };
        }

        private List<IScript> GetOrCreateScriptsListForId(string itemId) {
            if (scripts.TryGetValue(itemId, out var list)) {
                return list;
            } else {
                var initList = new List<IScript>();
                scripts.Add(itemId, initList);
                return initList;
            }
        }

    }
}
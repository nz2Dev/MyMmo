using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;

namespace MyMmo.Processing {
    public class Clip {

        private readonly Dictionary<string, List<BaseScriptData>> scripts = new Dictionary<string, List<BaseScriptData>>();
        private float changesDeltaTime;
        private float recordStartTime;
        private bool isRecording;

        public bool IsRecording => isRecording;
        
        public void RestartRecord(float startTime, float deltaTime) {
            recordStartTime = startTime;
            changesDeltaTime = deltaTime;
            isRecording = true;
            scripts.Clear();
        }
        
        public void AddChangesScript(string id, BaseScriptData script) {
            GetOrCreateList(id).Add(script);
        }

        private List<BaseScriptData> GetOrCreateList(string id) {
            if (scripts.TryGetValue(id, out var list)) {
                return list;
            } else {
                var initList = new List<BaseScriptData>();
                scripts.Add(id, initList);
                return initList;
            }
        }

        public ScriptsClipData ToData() {
            isRecording = false;
            return new ScriptsClipData {
                StartTime = recordStartTime,
                ChangesDeltaTime = changesDeltaTime, 
                ItemDataArray = scripts.Select(entry => new ItemScriptsData {
                    ItemId = entry.Key,
                    ScriptDataArray = entry.Value.ToArray()
                }).ToArray()
            };
        }
    }
}
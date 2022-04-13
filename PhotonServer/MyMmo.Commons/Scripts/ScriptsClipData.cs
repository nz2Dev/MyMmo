using System.Linq;
using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ScriptsClipData {

        [ProtoMember(1)]
        public ItemScriptsData[] ItemDataArray { get; set; } = new ItemScriptsData[0];

        [ProtoMember(2)]
        public float ChangesDeltaTime { get; set; } = 1f;

        [ProtoMember(3)]
        public float StartTime { get; set; }

        public float EndTime() {
            return StartTime + Duration();
        }

        public float Duration() {
            return ItemDataArray.Select(data => data.ScriptDataArray.Length).Max() * ChangesDeltaTime;
        }

        public float ScriptEndTime(int scriptIndex) {
            return StartTime + ScriptDuration(scriptIndex);
        }

        public float ScriptDuration(int scriptIndex) {
            var updateEndIndex = scriptIndex + 1;
            return ChangesDeltaTime * updateEndIndex;
        }
    }
}
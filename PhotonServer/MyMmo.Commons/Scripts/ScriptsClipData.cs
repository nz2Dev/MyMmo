using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ScriptsClipData {

        [ProtoMember(1)]
        public ItemScriptsData[] ItemDataArray { get; set; } = new ItemScriptsData[0];

    }
}
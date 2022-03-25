using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ScriptsClipData {

        [ProtoMember(1)]
        public ItemScriptsData[] ScriptsData { get; set; } = new ItemScriptsData[0];

    }
}
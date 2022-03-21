using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ScriptsDataClip {

        [ProtoMember(1)]
        public BaseScriptData[] ScriptsData { get; set; }

    }
}
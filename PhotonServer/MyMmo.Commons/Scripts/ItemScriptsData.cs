using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ItemScriptsData {

        [ProtoMember(1)] 
        public string ItemId { get; set; }

        [ProtoMember(2)]
        public BaseScriptData ItemScriptData { get; set; }

    }
}
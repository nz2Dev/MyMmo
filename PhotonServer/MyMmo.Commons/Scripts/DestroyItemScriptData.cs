using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class DestroyItemScriptData : BaseScriptData {

        [ProtoMember(1)]
        public string ItemId { get; set; }
    }
}
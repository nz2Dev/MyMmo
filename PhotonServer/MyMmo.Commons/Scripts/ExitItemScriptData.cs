using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ExitItemScriptData : BaseScriptData {

        [ProtoMember(1)]
        public string ItemId { get; set; }

        [ProtoMember(2)]
        public int FromLocationId { get; set; }
        
        [ProtoMember(3)]
        public int ToLocationId { get; set; }

    }
}
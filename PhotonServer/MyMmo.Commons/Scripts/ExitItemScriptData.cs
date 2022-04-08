using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ExitItemScriptData : BaseScriptData {

        [ProtoMember(1)]
        public string ItemId { get; set; }

    }
}
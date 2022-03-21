using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    [ProtoContract]
    public class ChangeLocationScriptData : BaseScriptData {

        [ProtoMember(1)]
        public string ItemId { get; set; }

        [ProtoMember(2)]
        public int FromLocation { get; set; }

        [ProtoMember(3)]
        public int ToLocation { get; set; }

    }
}
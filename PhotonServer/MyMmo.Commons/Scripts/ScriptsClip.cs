using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    [ProtoContract]
    public class ScriptsClip {

        [ProtoMember(1)]
        public ChangeLocationScript[] Scripts { get; set; }

    }
}
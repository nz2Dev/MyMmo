using MyMmo.Commons.Primitives;
using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    [ProtoContract]
    public class ChangePositionScriptData : BaseScriptData {

        [ProtoMember(1)]
        public Vector2 FromPosition { get; set; }

        [ProtoMember(2)]
        public Vector2 ToPosition { get; set; }

        [ProtoMember(3)]
        public string ItemId { get; set; }

        [ProtoMember(4)]
        public float Duration { get; set; }
        
        [ProtoMember(5)]
        public Vector2 Destination { get; set; }

    }
}
using MyMmo.Commons.Primitives;
using ProtoBuf;

namespace MyMmo.Commons.Snapshots {
    [ProtoContract]
    public class ItemSnapshotData {

        [ProtoMember(1)]
        public string ItemId { get; set; }

        [ProtoMember(2)]
        public int LocationId { get; set; }

        [ProtoMember(3)]
        public Vector2 PositionInLocation { get; set; }

    }
}
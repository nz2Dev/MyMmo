using ProtoBuf;

namespace MyMmo.Commons.Snapshots {
    
    [ProtoContract]
    public class LocationSnapshotData {

        [ProtoMember(1)]
        public ItemSnapshotData[] ItemsSnapshotData { get; set; } = new ItemSnapshotData[0];

        [ProtoMember(2)]
        public int LocationId { get; set; }

    }
}
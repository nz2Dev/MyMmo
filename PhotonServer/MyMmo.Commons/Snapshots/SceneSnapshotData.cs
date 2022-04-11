using ProtoBuf;

namespace MyMmo.Commons.Snapshots {
    
    [ProtoContract]
    public class SceneSnapshotData {

        [ProtoMember(1)]
        public EntitySnapshotData[] EntitiesSnapshotData { get; set; } = new EntitySnapshotData[0];

    }
}
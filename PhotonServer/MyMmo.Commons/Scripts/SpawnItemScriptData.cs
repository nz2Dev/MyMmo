using MyMmo.Commons.Snapshots;
using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    [ProtoContract]
    public class SpawnItemScriptData : BaseScriptData {
        
        [ProtoMember(1)]
        public ItemSnapshotData ItemSnapshotData { get; set; }
    }
}
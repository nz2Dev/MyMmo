using MyMmo.Commons;
using MyMmo.Commons.Snapshots;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class LocationEnterEvent {

        [PropertyKey(Key = (byte) ParameterCode.SerializedLocationSnapshot, IsOptional = false)]
        public byte[] LocationSnapshotBytes { get; set; }

        public LocationSnapshotData DeserializeLocationSnapshotData() {
            return SnapshotsDataProtocol.Deserialize(LocationSnapshotBytes);
        }

    }
}
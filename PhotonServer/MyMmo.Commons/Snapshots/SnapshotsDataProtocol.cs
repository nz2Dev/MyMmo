using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;

namespace MyMmo.Commons.Snapshots {
    public static class SnapshotsDataProtocol {

        public static TypeModel DeserializeTypeModel = RuntimeTypeModel.Default;
        
        public static byte[] Serialize(LocationSnapshotData snapshotData) {
            using (var stream = new MemoryStream()) {
                Serializer.Serialize(stream, snapshotData);
                return stream.ToArray();
            }
        }

        public static LocationSnapshotData Deserialize(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                return (LocationSnapshotData) DeserializeTypeModel.Deserialize(stream, null, typeof(LocationSnapshotData));
            }
        }

    }
}
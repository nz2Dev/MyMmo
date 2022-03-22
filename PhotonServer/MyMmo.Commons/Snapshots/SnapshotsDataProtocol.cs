using System.IO;
using ProtoBuf;

namespace MyMmo.Commons.Snapshots {
    public static class SnapshotsDataProtocol {

        public static byte[] Serialize(LocationSnapshotData snapshotData) {
            using (var stream = new MemoryStream()) {
                Serializer.Serialize(stream, snapshotData);
                return stream.ToArray();
            }
        }

        public static LocationSnapshotData Deserialize(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                return Serializer.Deserialize<LocationSnapshotData>(stream);
            }
        }

    }
}
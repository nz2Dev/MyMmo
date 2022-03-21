using System.IO;
using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    public static class ScriptsDataProtocol {

        public static byte[] Serialize(ScriptsDataClip scriptsDataClip) {
            using (var stream = new MemoryStream()) {
                Serializer.Serialize(stream, scriptsDataClip);
                return stream.ToArray();
            }
        }
        
        public static ScriptsDataClip Deserialize(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                return Serializer.Deserialize<ScriptsDataClip>(stream);
            }
        }

    }
}
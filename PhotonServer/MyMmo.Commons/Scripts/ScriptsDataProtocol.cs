using System.IO;
using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    public static class ScriptsDataProtocol {

        public static byte[] Serialize(ScriptsClip scriptsClip) {
            using (var stream = new MemoryStream()) {
                Serializer.Serialize(stream, scriptsClip);
                return stream.ToArray();
            }
        }
        
        public static ScriptsClip Deserialize(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                return Serializer.Deserialize<ScriptsClip>(stream);
            }
        }

    }
}
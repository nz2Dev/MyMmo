using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;

namespace MyMmo.Commons.Scripts {
    public static class ScriptsDataProtocol {

        public static TypeModel DeserializeTypeModel = RuntimeTypeModel.Default;

        public static byte[] Serialize(ScriptsDataClip scriptsDataClip) {
            using (var stream = new MemoryStream()) {
                Serializer.Serialize(stream, scriptsDataClip);
                return stream.ToArray();
            }
        }
        
        public static ScriptsDataClip Deserialize(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                return (ScriptsDataClip) DeserializeTypeModel.Deserialize(stream, null, typeof(ScriptsDataClip));
            }
        }

    }
}
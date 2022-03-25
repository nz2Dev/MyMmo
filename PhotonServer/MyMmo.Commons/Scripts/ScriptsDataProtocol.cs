using System.IO;
using ProtoBuf;
using ProtoBuf.Meta;

namespace MyMmo.Commons.Scripts {
    public static class ScriptsDataProtocol {

        public static TypeModel DeserializeTypeModel = RuntimeTypeModel.Default;

        public static byte[] Serialize(ScriptsClipData scriptsClipData) {
            using (var stream = new MemoryStream()) {
                Serializer.Serialize(stream, scriptsClipData);
                return stream.ToArray();
            }
        }
        
        public static ScriptsClipData Deserialize(byte[] data) {
            using (var stream = new MemoryStream(data)) {
                return (ScriptsClipData) DeserializeTypeModel.Deserialize(stream, null, typeof(ScriptsClipData));
            }
        }

    }
}
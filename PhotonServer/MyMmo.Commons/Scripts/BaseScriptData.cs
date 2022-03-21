using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    [ProtoInclude(1, typeof(ChangePositionScriptData))]
    [ProtoInclude(2, typeof(ChangeLocationScriptData))]
    public class BaseScriptData {
    }
}
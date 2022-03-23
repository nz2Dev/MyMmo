using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    [ProtoInclude(1, typeof(ChangePositionScriptData))]
    [ProtoInclude(2, typeof(ChangeLocationScriptData))]
    [ProtoInclude(3, typeof(SpawnItemScriptData))]
    [ProtoInclude(4, typeof(DestroyItemScriptData))]
    public class BaseScriptData {
    }
}
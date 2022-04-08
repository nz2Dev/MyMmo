using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    [ProtoInclude(1, typeof(ChangePositionScriptData))]
    [ProtoInclude(2, typeof(SpawnItemScriptData))]
    [ProtoInclude(3, typeof(DestroyItemScriptData))]
    [ProtoInclude(4, typeof(ExitItemScriptData))]
    [ProtoInclude(5, typeof(EnterItemScriptData))]
    public class BaseScriptData {
    }
}
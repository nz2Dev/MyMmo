using MyMmo.Commons;
using MyMmo.Commons.Scripts;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class RegionUpdateEvent {

        [PropertyKey(Key = (byte) ParameterCode.ScriptsList, IsOptional = false)]
        public ChangeLocationScript[] Scripts { get; set; }

        [PropertyKey(Key = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; set; }
        
    }
}
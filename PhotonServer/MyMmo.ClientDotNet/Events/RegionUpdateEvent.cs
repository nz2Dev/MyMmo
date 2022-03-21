using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class RegionUpdateEvent {

        [PropertyKey(Key = (byte) ParameterCode.SerializedScripts, IsOptional = false)]
        public byte[] ScriptsBytes { get; set; }

        [PropertyKey(Key = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; set; }
        
    }
}
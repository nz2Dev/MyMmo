using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class LocationExitEvent {
        
        [PropertyKey(Key = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; set; }
        
    }
}
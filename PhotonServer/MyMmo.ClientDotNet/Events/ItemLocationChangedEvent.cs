using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class ItemLocationChangedEvent {

        [PropertyKey(key: (byte) ParameterCode.ItemId, optional: false)]
        public string ItemId { get; set; }
        
        [PropertyKey(Key = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; set; }

    }
}
using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class ItemSubscribedEvent {

        [PropertyKey(key: (byte) ParameterCode.ItemId, optional: false)]
        public string ItemId { get; set; }

        [PropertyKey(key: (byte) ParameterCode.LocationId, optional: false)]
        public int LocationId { get; set; }

    }
}
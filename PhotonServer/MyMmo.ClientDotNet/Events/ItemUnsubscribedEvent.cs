using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class ItemUnsubscribedEvent {

        [PropertyKey(key: (byte) ParameterCode.ItemId, optional: false)]
        public string ItemId { get; set; }

    }
}
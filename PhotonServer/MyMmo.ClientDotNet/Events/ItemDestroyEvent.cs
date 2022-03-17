using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Events {
    public class ItemDestroyEvent {

        [PropertyKey(Key = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string ItemId { get; set; }

    }
}
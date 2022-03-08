using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.ConsoleClient.Data {
    public class ItemExitEvent {

        [PropertyKey(key: (byte) ParameterCode.ItemId, optional: false)]
        public string ItemId { get; set; }

    }
}
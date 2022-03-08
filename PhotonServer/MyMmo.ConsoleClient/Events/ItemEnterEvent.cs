using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.ConsoleClient.Data {
    public class ItemEnterEvent {

        [PropertyKey(key: (byte) ParameterCode.ItemId, optional: false)]
        public string ItemId { get; set; }

        [PropertyKey(key: (byte) ParameterCode.LocationId, optional: false)]
        public int LocationId { get; set; }

    }
}
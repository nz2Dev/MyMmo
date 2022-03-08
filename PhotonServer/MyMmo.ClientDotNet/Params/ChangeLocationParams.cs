using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Params {
    public class ChangeLocationParams {

        [PropertyKey(Key = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string ItemId { get; set; }
        
        [PropertyKey(Key = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; set; }

    }
}
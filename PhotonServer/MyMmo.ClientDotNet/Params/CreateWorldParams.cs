using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client.Params {
    public class CreateWorldParams {

        [PropertyKey(key: (byte) ParameterCode.WorldName, optional: false)]
        public string WorldName { get; set; }
        
    }
}
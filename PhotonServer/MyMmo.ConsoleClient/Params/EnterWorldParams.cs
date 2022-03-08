using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.ConsoleClient.Params {
    public class EnterWorldParams {

        [PropertyKey(key: (byte) ParameterCode.WorldName, optional: false)]
        public string WorldName { get; set; }

        [PropertyKey(key: (byte) ParameterCode.UserName, optional: false)]
        public string UserName { get; set; }
        
    }
}
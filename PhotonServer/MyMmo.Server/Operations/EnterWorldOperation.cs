using MyMmo.Commons;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Operations {
    public class EnterWorldOperation : Operation {

        public EnterWorldOperation(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte) ParameterCode.UserName, IsOptional = false)]
        public string UserName { get; set; }
        
        [DataMember(Code = (byte) ParameterCode.WorldName, IsOptional = false)]
        public string WorldName { get; set; }

    }
}
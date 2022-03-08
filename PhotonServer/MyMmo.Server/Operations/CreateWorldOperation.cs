using MyMmo.Commons;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Operations {
    public class CreateWorldOperation : Operation {

        public CreateWorldOperation(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) {
        }

        [DataMember(Code = (byte) ParameterCode.WorldName, IsOptional = false)]
        public string WorldName { get; set; }

    }
}
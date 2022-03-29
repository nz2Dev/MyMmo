using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Operations {
    public class MoveAvatarRandomlyOperation : Operation {

        public MoveAvatarRandomlyOperation(IRpcProtocol protocol, OperationRequest request) : base(protocol, request) {
        }
    }
}
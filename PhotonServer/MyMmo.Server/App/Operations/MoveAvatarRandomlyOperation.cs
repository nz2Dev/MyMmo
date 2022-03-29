using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.App.Operations {
    public class MoveAvatarRandomlyOperation : Operation {

        public MoveAvatarRandomlyOperation(IRpcProtocol protocol, OperationRequest request) : base(protocol, request) {
        }
    }
}
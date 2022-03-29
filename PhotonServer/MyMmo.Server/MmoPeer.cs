using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server {
    
    public class MmoPeer : Peer {

        private readonly MmoInitialOperationsHandler initialOperationsHandler = new MmoInitialOperationsHandler();
        
        public MmoPeer(InitRequest initRequest) : base(initRequest) {
            SetInitialOperationHandler();
        }

        private void SetInitialOperationHandler() {
            SetCurrentOperationHandler(initialOperationsHandler);
        }
    }
}
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace ServerV5WebSocket {
    public class ServerV5Peer : Peer {

        public ServerV5Peer(InitRequest initRequest) : base(initRequest) {
        }

        protected override void OnDisconnect(int reasonCode, string reasonDetail) {
            base.OnDisconnect(reasonCode, reasonDetail);
        }

    }
}
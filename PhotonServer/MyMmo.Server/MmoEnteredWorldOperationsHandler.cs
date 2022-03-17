using System;
using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Server.Operations;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server {
    public class MmoEnteredWorldOperationsHandler : IOperationHandler {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly Item avatarItem;
        private readonly InterestArea interestArea;

        public MmoEnteredWorldOperationsHandler(Item avatarItem, InterestArea interestArea) {
            this.avatarItem = avatarItem;
            this.interestArea = interestArea;
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest,
            SendParameters sendParameters) {
            switch ((OperationCode) operationRequest.OperationCode) {
                case OperationCode.ChangeLocation: {
                    return OperationChangeLocation(peer, operationRequest, sendParameters);
                }

                case OperationCode.CreateWorld:
                case OperationCode.EnterWorld:
                    return MmoOperationsUtils.OperationWrongState(operationRequest);
                
                default:
                    return MmoOperationsUtils.OperationNotSupported(operationRequest);
            }
        }

        private OperationResponse OperationChangeLocation(PeerBase peer, OperationRequest operationRequest,
            SendParameters sendParameters) {
            var operationChangeLocation = new ChangeLocationOperation(peer.Protocol, operationRequest);
            if (!operationChangeLocation.IsValid) {
                return MmoOperationsUtils.OperationWrongDataContract(operationRequest, operationChangeLocation);
            }

            avatarItem.ChangeLocation(operationChangeLocation.LocationId);
            
            return MmoOperationsUtils.OperationSuccess(operationRequest);
        }

        public void OnDisconnect(PeerBase peer) {
            logger.Info($"entered world operation handler of avatar {avatarItem.Id} is going to disconnect");
            
            avatarItem.Destroy();
            avatarItem.Dispose();
            interestArea.Dispose();
            
            ((Peer) peer).SetCurrentOperationHandler(null);
            peer.Dispose();
        }

    }
}
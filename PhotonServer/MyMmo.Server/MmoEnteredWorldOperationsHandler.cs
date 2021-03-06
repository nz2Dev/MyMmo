using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Processing.Processes;
using MyMmo.Server.Domain;
using MyMmo.Server.Operations;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server {
    public class MmoEnteredWorldOperationsHandler : IOperationHandler {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly World world;
        private readonly Item avatarItem;
        private readonly InterestArea interestArea;

        public MmoEnteredWorldOperationsHandler(Item avatarItem, InterestArea interestArea, World world) {
            this.avatarItem = avatarItem;
            this.interestArea = interestArea;
            this.world = world;
        }

        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest,
            SendParameters sendParameters) {
            switch ((OperationCode) operationRequest.OperationCode) {
                case OperationCode.ChangeLocation: {
                    return OperationChangeLocation(peer, operationRequest, sendParameters);
                }

                case OperationCode.MoveAvatarRandomly: {
                    return OperationMoveAvatarRandomly(peer, operationRequest, sendParameters);
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
            
            var avatarLocation = world.GetLocation(avatarItem.LocationId);
            var newLocation = world.GetLocation(operationChangeLocation.LocationId);
            avatarLocation.RequestProcess(new ExitToLocationProcess(avatarItem.Id, newLocation.Id));

            return MmoOperationsUtils.OperationSuccess(operationRequest);
        }

        private OperationResponse OperationMoveAvatarRandomly(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters) {
            var operationMove = new MoveAvatarRandomlyOperation(peer.Protocol, operationRequest);
            if (!operationMove.IsValid) {
                return MmoOperationsUtils.OperationWrongDataContract(operationRequest, operationMove);
            }
            
            var avatarLocation = world.GetLocation(avatarItem.LocationId);
            avatarLocation.RequestProcess(new MoveItemRandomlyProcess(avatarItem.Id));

            return MmoOperationsUtils.OperationSuccess(operationRequest);
        }

        public void OnDisconnect(PeerBase peer) {
            logger.Info($"entered world operation handler of avatar {avatarItem} is going to disconnect");
            
            var avatarLocation = world.GetLocation(avatarItem.LocationId);
            avatarLocation.RequestProcess(new DestroyItemProcess(avatarItem.Id));
            interestArea.Dispose();
            
            ((Peer) peer).SetCurrentOperationHandler(null);
            peer.Dispose();
        }

    }
}
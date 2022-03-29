using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Server.Game;
using MyMmo.Server.Game.Writers;
using MyMmo.Server.Operations;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server {
    public class MmoEnteredWorldOperationsHandler : IOperationHandler {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly World world;
        private readonly string avatarItemId;
        private readonly InterestArea interestArea;

        public MmoEnteredWorldOperationsHandler(string avatarItemId, InterestArea interestArea, World world) {
            this.avatarItemId = avatarItemId;
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

            var avatarItem = world.GetItem(avatarItemId);
            var avatarLocation = world.GetLocation(avatarItem.LocationId);
            avatarLocation.RequestProducer(new ChangeLocationWriter(avatarItem.Id, operationChangeLocation.LocationId));

            return MmoOperationsUtils.OperationSuccess(operationRequest);
        }

        private OperationResponse OperationMoveAvatarRandomly(PeerBase peer, OperationRequest operationRequest, SendParameters sendParameters) {
            var operationMove = new MoveAvatarRandomlyOperation(peer.Protocol, operationRequest);
            if (!operationMove.IsValid) {
                return MmoOperationsUtils.OperationWrongDataContract(operationRequest, operationMove);
            }

            var avatarItem = world.GetItem(avatarItemId);
            var avatarLocation = world.GetLocation(avatarItem.LocationId);
            avatarLocation.RequestProducer(new MoveItemRandomlyWriter(avatarItem.Id));

            return MmoOperationsUtils.OperationSuccess(operationRequest);
        }

        public void OnDisconnect(PeerBase peer) {
            logger.Info($"entered world operation handler of avatar {avatarItemId} is going to disconnect");
            
            var avatarItem = world.GetItem(avatarItemId);
            var avatarLocation = world.GetLocation(avatarItem.LocationId);
            avatarLocation.RequestProducer(new DestroyItemWriter(avatarItem.Id));
            interestArea.Dispose();
            
            ((Peer) peer).SetCurrentOperationHandler(null);
            peer.Dispose();
        }

    }
}
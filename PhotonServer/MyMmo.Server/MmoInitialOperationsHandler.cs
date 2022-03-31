using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Server.Game;
using MyMmo.Server.Game.Updates;
using MyMmo.Server.Operations;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server {
    public class MmoInitialOperationsHandler : IOperationHandler {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger(); 
        
        public OperationResponse OnOperationRequest(PeerBase peer, OperationRequest operationRequest,
            SendParameters sendParameters) {
            switch ((OperationCode) operationRequest.OperationCode) {
                case OperationCode.EnterWorld: {
                    return OperationEnterWorld(peer, operationRequest, sendParameters);
                }

                case OperationCode.CreateWorld: {
                    return OperationCreateWorld(peer, operationRequest, sendParameters);
                }

                case OperationCode.ChangeLocation:
                    return MmoOperationsUtils.OperationWrongState(operationRequest);

                default:
                    return MmoOperationsUtils.OperationNotSupported(operationRequest);
            }
        }

        private OperationResponse OperationCreateWorld(PeerBase peer, OperationRequest operationRequest,
            SendParameters sendParameters) {
            var createWorldOperation = new CreateWorldOperation(peer.Protocol, operationRequest);
            if (!createWorldOperation.IsValid) {
                return MmoOperationsUtils.OperationWrongDataContract(operationRequest, createWorldOperation);
            }

            if (!WorldCache.Instance.TryCreate(createWorldOperation.WorldName, World.CreateDefaultWorld)) {
                return MmoOperationsUtils.OperationError(
                    operationRequest,
                    ReturnCode.WorldAlreadyExist,
                    $"World {createWorldOperation.WorldName} already exist"
                );
            }

            return MmoOperationsUtils.OperationSuccess(
                operationRequest
            );
        }

        private OperationResponse OperationEnterWorld(PeerBase peer, OperationRequest operationRequest,
            SendParameters sendParameters) {
            var enterWorldOperation = new EnterWorldOperation(peer.Protocol, operationRequest);
            if (!enterWorldOperation.IsValid) {
                return MmoOperationsUtils.OperationWrongDataContract(operationRequest, enterWorldOperation);
            }

            World world;
            if (!WorldCache.Instance.TryGet(enterWorldOperation.WorldName, out world)) {
                return MmoOperationsUtils.OperationError(
                    operationRequest,
                    ReturnCode.WorldNotFound,
                    $"World {enterWorldOperation.WorldName} is not found"
                );
            }

            var avatarItem = new Item(enterWorldOperation.UserName, peer);
            var spawnLocation = world.GetLocation(World.RootLocationId);
            var spawnAvatarUpdate = new SpawnClientAvatarUpdate(avatarItem, spawnLocation.Id);
            if (!spawnAvatarUpdate.IsValidAt(world)) {
                return MmoOperationsUtils.OperationError(
                    operationRequest,
                    ReturnCode.AvatarRegistrationError,
                    $"Can't register Avatar, it's already exist, specified UserName: {enterWorldOperation.UserName}"
                );
            }

            var interestArea = new ClientInterestArea(peer, world, enterWorldOperation.UserName);
            interestArea.FollowLocationOf(avatarItem);
            interestArea.WatchLocationManually(spawnLocation.Id);
            interestArea.EnqueueInLocationChangingFiber(() => {
                spawnLocation.RequestUpdate(spawnAvatarUpdate);
            });

            var enteredWorldOperationHandler = new MmoEnteredWorldOperationsHandler(avatarItem, interestArea, world);
            ((Peer) peer).SetCurrentOperationHandler(enteredWorldOperationHandler);
            
            var responseParams = new EnterWorldResponseParams {AvatarItemId = avatarItem.Id};
            return MmoOperationsUtils.OperationSuccess(operationRequest, responseParams);
        }

        public void OnDisconnect(PeerBase peer) {
            logger.Info($"initial operation handler of peer with connection id {peer.ConnectionId} is going to disconnect");
            ((Peer) peer).SetCurrentOperationHandler(null);
            peer.Dispose();
        }

    }
}
using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Server.Operations;
using MyMmo.Server.Producers;
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

            var spawnLocation = world.GetLocation(World.RootLocationId);
            var interestArea = new ClientInterestArea(peer, world, enterWorldOperation.UserName);
            var spawnItemProducer = new SpawnClientAvatarProducer(enterWorldOperation.UserName, spawnLocation.Id, interestArea, peer);
            if (!spawnItemProducer.IsValidAt(world)) {
                interestArea.Dispose();
                return MmoOperationsUtils.OperationError(
                    operationRequest,
                    ReturnCode.AvatarRegistrationError,
                    $"Can't register Avatar, it's already exist, specified UserName: {enterWorldOperation.UserName}"
                );
            }
            
            interestArea.WatchLocationManually(spawnLocation.Id, snapshot => {
                // every entered location during one time manual management will callback with it's snapshot
                if (snapshot.Source.Id == spawnLocation.Id) { 
                    // we wait to make RequestSpawn until our target location callback,
                    // because there is a chance that location will consume our spawn request before state construction
                    spawnLocation.RequestSpawnItem(spawnItemProducer);        
                }
            });
            
            var enteredWorldOperationHandler = new MmoEnteredWorldOperationsHandler(spawnItemProducer.itemId, interestArea, world);
            ((Peer) peer).SetCurrentOperationHandler(enteredWorldOperationHandler);
            
            var responseParams = new EnterWorldResponseParams {AvatarItemId = spawnItemProducer.itemId};
            return MmoOperationsUtils.OperationSuccess(operationRequest, responseParams);
        }

        public void OnDisconnect(PeerBase peer) {
            logger.Info($"initial operation handler of peer with connection id {peer.ConnectionId} is going to disconnect");
            ((Peer) peer).SetCurrentOperationHandler(null);
            peer.Dispose();
        }

    }
}
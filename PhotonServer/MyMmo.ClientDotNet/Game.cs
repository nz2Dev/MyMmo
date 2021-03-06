using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using MyMmo.Client.Events;
using MyMmo.Client.Params;
using MyMmo.Client.Response;
using MyMmo.Commons;
using MyMmo.Commons.Scripts;
using MyMmo.Playground;

namespace MyMmo.Client {
    public class Game : IPhotonPeerListener {

        private PhotonPeer peer;
        private readonly IGameListener listener;
        private DebugLevel debugLevel;
        private string avatarId;
        
        public Game(IGameListener listener) {
            this.listener = listener;
        }

        public string AvatarId => avatarId;

        public void Initialize(PhotonPeer photonPeer, DebugLevel internalDebugLevel = DebugLevel.ERROR) {
            peer = photonPeer;
            debugLevel = internalDebugLevel;
        }

        public void ConnectDefault() {
            Connect("6.tcp.ngrok.io:17091");
        }

        public void ConnectLocal() {
            Connect("localhost:4530");
        }
        
        public void Connect(string serverAddress) {
            DebugReturn(DebugLevel.INFO, $"Trying to connect to MyMmoServerApp at {serverAddress}");
            DebugReturn(DebugLevel.INFO, $"peer connection result: {peer.Connect(serverAddress, "MyMmoServerApp")}");
        }

        public void Update() {
            peer?.Service();
        }

        public void CreateWorld(CreateWorldParams createWorldParams) {
            peer.SendOperation((byte) OperationCode.CreateWorld, EventDataConverter.ToDictionary(createWorldParams), SendOptions.SendReliable);
        }

        public void EnterWorld(EnterWorldParams enterWorldParams) {
            peer.SendOperation((byte) OperationCode.EnterWorld, EventDataConverter.ToDictionary(enterWorldParams), SendOptions.SendReliable);
        }

        public void ChangeLocation(string itemId, int locationId) {
            var changeLocationParams = new ChangeLocationParams {ItemId = itemId, LocationId = locationId};
            peer.SendOperation((byte) OperationCode.ChangeLocation, EventDataConverter.ToDictionary(changeLocationParams),
                SendOptions.SendReliable);
        }

        public void MoveAvatarRandomly() {
            peer.SendOperation((byte) OperationCode.MoveAvatarRandomly, new Dictionary<byte, object>(), SendOptions.SendUnreliable);
        }

        public void DebugReturn(DebugLevel level, string message) {
            listener.OnLog(level, message);
            if (level <= debugLevel) {
                Console.WriteLine($"{level}: {message}");
            }
        }

        public void OnOperationResponse(OperationResponse operationResponse) {
            DebugReturn(DebugLevel.INFO, "operationResponse: " + operationResponse.ToStringFull());
            if (operationResponse.ReturnCode == (short) ReturnCode.Ok) {
                switch ((OperationCode) operationResponse.OperationCode) {
                    case OperationCode.CreateWorld: {
                        listener.OnWorldCreated();
                        break;
                    }

                    case OperationCode.EnterWorld: {
                        var enterWorldResponse =
                            EventDataConverter.Convert<EnterWorldResponse>(operationResponse.Parameters.paramDict);
                        avatarId = enterWorldResponse.AvatarItemId;
                        listener.OnWorldEntered();
                        break;
                    }
                }
            } else if ((OperationCode) operationResponse.OperationCode == OperationCode.CreateWorld
                       && (ReturnCode) operationResponse.ReturnCode == ReturnCode.WorldAlreadyExist) {
                listener.OnWorldCreated();
            } else {
                DebugReturn(DebugLevel.ERROR,
                    $"response error {operationResponse.ReturnCode}: {operationResponse.DebugMessage}");
            }
        }

        public void OnStatusChanged(StatusCode statusCode) {
            DebugReturn(DebugLevel.INFO, "statusChanged: " + statusCode);
            switch (statusCode) {
                case StatusCode.Connect: {
                    listener.OnConnected();
                    break;
                }
            }
        }

        public void OnEvent(EventData eventData) {
            DebugReturn(DebugLevel.INFO, "event: " + eventData.ToStringFull());
            switch ((EventCode) eventData.Code) {
                case EventCode.LocationEnterEvent: {
                    var enterEvent = EventDataConverter.Convert<LocationEnterEvent>(eventData.Parameters.paramDict);
                    var locationSnapshotData = enterEvent.DeserializeLocationSnapshotData();
                    listener.OnLocationEntered(enterEvent.LocationId, locationSnapshotData);
                    break;
                }

                case EventCode.LocationExitEvent: {
                    var exitEvent = EventDataConverter.Convert<LocationExitEvent>(eventData.Parameters.paramDict);
                    listener.OnLocationExit(exitEvent.LocationId);
                    break;
                }

                case EventCode.LocationUpdated: {
                    var regionUpdateEvent =
                        EventDataConverter.Convert<LocationUpdateEvent>(eventData.Parameters.paramDict);
                    var scriptsClip = ScriptsDataProtocol.Deserialize(regionUpdateEvent.ScriptsBytes);
                    listener.OnLocationUpdate(regionUpdateEvent.LocationId, scriptsClip);
                    break;
                }
            }
        }

        public void OnMessage(object messages) {
            DebugReturn(DebugLevel.INFO, "message: " + messages);
        }

    }
}
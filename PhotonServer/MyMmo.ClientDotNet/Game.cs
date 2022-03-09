using System.Collections.Generic;
using ExitGames.Client.Photon;
using MyMmo.Client.Events;
using MyMmo.Client.Params;
using MyMmo.Client.Response;
using MyMmo.Commons;
using MyMmo.Playground;

namespace MyMmo.Client {
    public class Game : IPhotonPeerListener {

        private PhotonPeer peer;
        private readonly IGameListener listener;
        private readonly Dictionary<string, Item> itemCache = new Dictionary<string, Item>();
        private DebugLevel debugLevel;

        private string avatarId;

        public Game(IGameListener listener) {
            this.listener = listener;
        }

        public void Initialize(PhotonPeer photonPeer, DebugLevel internalDebugLevel = DebugLevel.ERROR) {
            peer = photonPeer;
            debugLevel = internalDebugLevel;
        }

        public ICollection<Item> Items => itemCache.Values;
        public Item AvatarItem => string.IsNullOrEmpty(avatarId) ? null : itemCache[avatarId];

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
            peer.Service();
        }

        public void CreateWorld(CreateWorldParams createWorldParams) {
            peer.OpCustom((byte) OperationCode.CreateWorld, EventDataConverter.ToDictionary(createWorldParams), true);
        }

        public void EnterWorld(EnterWorldParams enterWorldParams) {
            peer.OpCustom((byte) OperationCode.EnterWorld, EventDataConverter.ToDictionary(enterWorldParams), true);
        }

        public void ChangeLocation(string itemId, int locationId) {
            var changeLocationParams = new ChangeLocationParams {ItemId = itemId, LocationId = locationId};
            peer.OpCustom((byte) OperationCode.ChangeLocation, EventDataConverter.ToDictionary(changeLocationParams),
                true);
        }

        public void DebugReturn(DebugLevel level, string message) {
            listener.OnLog(level, message);
            if (level <= debugLevel) {
                System.Console.WriteLine($"{level}: {message}");
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
                            EventDataConverter.Convert<EnterWorldResponse>(operationResponse.Parameters);
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
                case EventCode.ItemEnterEvent: {
                    var enterEvent = EventDataConverter.Convert<ItemEnterEvent>(eventData.Parameters);
                    var item = new Item(enterEvent.ItemId, enterEvent.LocationId);
                    itemCache.Add(item.Id, item);
                    listener.OnItemEnter(item);
                    break;
                }

                case EventCode.ItemExitEvent: {
                    var exitEvent = EventDataConverter.Convert<ItemExitEvent>(eventData.Parameters);
                    itemCache.Remove(exitEvent.ItemId);
                    listener.OnItemExit(exitEvent.ItemId);
                    break;
                }

                case EventCode.ItemLocationChanged: {
                    var locationChangedEvent =
                        EventDataConverter.Convert<ItemLocationChangedEvent>(eventData.Parameters);
                    if (itemCache.TryGetValue(locationChangedEvent.ItemId, out var item)) {
                        item.ChangeLocationId(locationChangedEvent.LocationId);
                    }

                    listener.OnItemLocationChanged(item);
                    break;
                }
            }
        }

        public void OnMessage(object messages) {
            DebugReturn(DebugLevel.INFO, "message: " + messages);
        }

    }
}
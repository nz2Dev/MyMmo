using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using MyMmo.Client.Consumers;
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
        private readonly Dictionary<string, Item> itemCache = new Dictionary<string, Item>();
        private DebugLevel debugLevel;

        private string avatarId;
        private readonly ChangeLocationClientScriptReader scriptReader;

        public Game(IGameListener listener) {
            this.listener = listener;
            scriptReader = new ChangeLocationClientScriptReader(itemCache);
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

        public void ApplyPerformedScript(ChangeLocationScript script) {
            // modify internal state
            // todo move internal state to it's ClientWorld domain
            // that would represent internal state for scripts to take last state information
            scriptReader.ApplyScript(script);
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
                case EventCode.ItemSubscribedEvent: {
                    var subscribeEvent = EventDataConverter.Convert<ItemSubscribedEvent>(eventData.Parameters.paramDict);
                    Item item;
                    if (itemCache.TryGetValue(subscribeEvent.ItemId, out var itemInCache)) {
                        itemInCache.LocationId = subscribeEvent.LocationId;
                        item = itemInCache;
                    } else {
                        var newItem = new Item(subscribeEvent.ItemId, subscribeEvent.LocationId);
                        itemCache.Add(newItem.Id, newItem);
                        item = newItem;
                    }
                    listener.OnItemSubscribed(item);
                    break;
                }

                case EventCode.ItemUnsubscribedEvent: {
                    var unsubscribedEvent = EventDataConverter.Convert<ItemUnsubscribedEvent>(eventData.Parameters.paramDict);
                    if (itemCache.TryGetValue(unsubscribedEvent.ItemId, out var itemInCache)) {
                        listener.OnItemUnsubscribed(itemInCache);
                    }
                    break;
                }

                case EventCode.ItemLocationChanged: {
                    var locationChangedEvent =
                        EventDataConverter.Convert<ItemLocationChangedEvent>(eventData.Parameters.paramDict);
                    if (itemCache.TryGetValue(locationChangedEvent.ItemId, out var item)) {
                        item.LocationId = locationChangedEvent.LocationId;
                    }

                    listener.OnItemLocationChanged(item);
                    break;
                }

                case EventCode.ItemDestroyEvent: {
                    var itemDestroyEvent = EventDataConverter.Convert<ItemDestroyEvent>(eventData.Parameters.paramDict);
                    if (itemCache.TryGetValue(itemDestroyEvent.ItemId, out var itemInCache)) {
                        itemInCache.IsDestroyed = itemCache.Remove(itemDestroyEvent.ItemId);
                        listener.OnItemDestroyed(itemInCache);
                    }
                    break;
                }

                case EventCode.RegionUpdated: {
                    var regionUpdateEvent =
                        EventDataConverter.Convert<RegionUpdateEvent>(eventData.Parameters.paramDict);
                    var scriptsClip = ScriptsDataProtocol.Deserialize(regionUpdateEvent.ScriptsBytes);
                    listener.OnRegionUpdate(regionUpdateEvent.LocationId, scriptsClip.Scripts);
                    break;
                }
            }
        }

        public void OnMessage(object messages) {
            DebugReturn(DebugLevel.INFO, "message: " + messages);
        }

    }
}
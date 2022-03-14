using System;
using System.Diagnostics.CodeAnalysis;
using ExitGames.Concurrency.Channels;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Server.Events;
using Photon.SocketServer;

namespace MyMmo.Server {
    public class Item : IDisposable {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly string id;
        private readonly World world;
        private readonly PeerBase owner;

        private readonly Channel<ItemEventMessage> itemEventChannel = new Channel<ItemEventMessage>();
        private readonly Channel<ItemLocationChangedMessage> locationChangedChannel = new Channel<ItemLocationChangedMessage>();
        private readonly Channel<ItemDisposedMessage> disposeChannel = new Channel<ItemDisposedMessage>();

        private int locationId;
        private Region currentRegion;
        private IDisposable regionSubscription;
        private bool disposed;

        public Item(string id, PeerBase owner, World world) {
            this.id = id;
            this.owner = owner;
            this.world = world;
        }

        ~Item() {
            Dispose(false);
        }

        public string Id => id;
        public Region CurrentRegion => currentRegion;
        public int LocationId => locationId;
        public bool Disposed => disposed;

        public void SendEvent(EventData eventData, SendParameters sendParameters = new SendParameters()) {
            var eventMessage = new ItemEventMessage(eventData, this, sendParameters);
            itemEventChannel.Publish(eventMessage);
        }

        public void ChangeLocation(int locationId) {
            this.locationId = locationId;

            logger.Info($"item {id} publish ItemEvent with changed location {locationId}");
            var locationChangedData = new ItemLocationChangedData {ItemId = id, LocationId = locationId};
            SendEvent(new EventData((byte) EventCode.ItemLocationChanged, locationChangedData));

            logger.Info($"item {id} start interest management update");
            UpdateInterestManagement();

            logger.Info($"item {id} publish ItemLocationChanged {locationId}");
            locationChangedChannel.Publish(new ItemLocationChangedMessage(locationId));
        }

        public IDisposable
            SubscribeLocationChanged(IFiber fiber, Action<ItemLocationChangedMessage> onLocationChanged) {
            return locationChangedChannel.Subscribe(fiber, onLocationChanged);
        }

        public IDisposable SubscribeOnDispose(IFiber fiber, Action<ItemDisposedMessage> onDisposed) {
            return disposeChannel.Subscribe(fiber, onDisposed);
        }

        private void UpdateInterestManagement() {
            var oldRegion = currentRegion;
            var newRegion = world.GetRegion(locationId);
            if (oldRegion == newRegion) {
                return;
            }

            // this is for when item moves
            logger.Info($"item {id} is going to update interest management");
            var itemRegionChangedMessage = new ItemRegionChangedMessage(oldRegion, newRegion, NewSnapshot());
            logger.Info($"item {id} publish ItemRegionChanged {oldRegion?.Id} -> {newRegion.Id} to its oldRegion {oldRegion?.Id}");
            oldRegion?.PublishItemRegionChanged(itemRegionChangedMessage);
            logger.Info($"item {id} publish ItemRegionChanged {oldRegion?.Id} -> {newRegion.Id} to its newRegion {newRegion.Id} ");
            newRegion.PublishItemRegionChanged(itemRegionChangedMessage);

            currentRegion = newRegion;

            // this is for when interest areas moves
            logger.Info($"item {id} disposes subscription to region {oldRegion?.Id}' RequestItemSnapshot, new publishes from it will not be received");
            logger.Info($"item {id} disposes subscription to its ItemEvent that transmitted messages to region {oldRegion?.Id}");
            regionSubscription?.Dispose();
            logger.Info($"item {id} subscribes to region {newRegion.Id}' RequestItemSnapshot, now it's a resident of that region");
            logger.Info($"item {id} subscribes to its ItemEvent to transmit messages to its new region {newRegion.Id}");
            regionSubscription = new SubscriptionsCollection(
                newRegion.SubscribeRequestItemSnapshot(owner.RequestFiber, message => {
                    logger.Info($"item {id} receive RequestItemSnapshot, region {newRegion.Id} request our snapshot, " +
                                "calling back with it");
                    message.SnapshotCallback.Invoke(NewSnapshot());
                }),
                itemEventChannel.Subscribe(owner.RequestFiber, message => {
                    logger.Info($"item {id} receive ItemEventMessage on peer fiber and routes it to its " +
                                $"current region {newRegion.Id} by publishing received message to that region");
                    newRegion.PublishItemEvent(message);
                })
            );
        }

        public void Dispose() {
            logger.Info($"item {id} is going to dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private void Dispose(bool disposing) {
            if (disposing) {
                disposed = true;
                currentRegion = null;
                regionSubscription?.Dispose();
                disposeChannel.Publish(new ItemDisposedMessage(this));
            }
        }

        private Snapshot NewSnapshot() {
            return new Snapshot(id, locationId, this);
        }

        public class Snapshot {

            public string Id { get; }
            public int LocationId { get; }
            public Item Source { get; }

            public Snapshot(string id, int locationId, Item source) {
                Id = id;
                LocationId = locationId;
                Source = source;
            }

        }

    }
}
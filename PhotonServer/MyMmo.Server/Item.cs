using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ExitGames.Concurrency.Channels;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using Photon.SocketServer;

namespace MyMmo.Server {
    public class Item : IDisposable {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly string id;
        private readonly PeerBase owner;

        private readonly Channel<ItemLocationChangedMessage> locationChangedChannel = new Channel<ItemLocationChangedMessage>();
        private readonly Channel<ItemDisposedMessage> disposeChannel = new Channel<ItemDisposedMessage>();

        private int locationId;
        private bool disposed;
        private bool destroyed;
        private bool spawned;

        private Vector2 positionInLocation;

        public Item(string id, PeerBase owner) {
            this.id = id;
            this.owner = owner;
        }

        ~Item() {
            Dispose(false);
        }

        public string Id => id;
        public int LocationId => locationId;
        public bool Disposed => disposed;
        public bool Destroyed => destroyed;
        public PeerBase Owner => owner;
        public bool Spawned => spawned;
        public Vector2 PositionInLocation => positionInLocation;

        public void Spawn(int spawnLocationId, Vector2 positionState) {
            spawned = true;
            ChangeLocation(spawnLocationId);
            ChangePositionInLocation(positionState);
        }
        
        public void ChangeLocation(int newLocationId) {
            locationId = newLocationId;
            locationChangedChannel.Publish(new ItemLocationChangedMessage(newLocationId));
        }

        public IDisposable SubscribeLocationChanged(IFiber fiber, Action<ItemLocationChangedMessage> onLocChanged) {
            return locationChangedChannel.Subscribe(fiber, onLocChanged);
        }
        
        public void ChangePositionInLocation(Vector2 vector2) {
            positionInLocation = vector2;
        }

        // todo implement destruction state script
        public void Destroy() {
            destroyed = true;
        }

        public ItemSnapshot GenerateItemSnapshot() {
            return new ItemSnapshot(id, locationId, positionInLocation);
        }

        public void Dispose() {
            logger.Info($"item {id} is going to dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDisposable SubscribeOnDispose(IFiber fiber, Action<ItemDisposedMessage> onDisposed) {
            return disposeChannel.Subscribe(fiber, onDisposed);
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private void Dispose(bool disposing) {
            if (disposing) {
                disposed = true;
                disposeChannel.Publish(new ItemDisposedMessage(this));
            }
        }
    }
}
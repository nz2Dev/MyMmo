using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using ExitGames.Concurrency.Channels;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using Photon.SocketServer;

namespace MyMmo.Server.Domain {
    // todo change itemId type to int everywhere, no needs for string
    public class Item : IDisposable {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly Channel<ItemLocationChangedMessage> locationChangedChannel = new Channel<ItemLocationChangedMessage>();
        private readonly Channel<ItemDisposedMessage> disposeChannel = new Channel<ItemDisposedMessage>();

        public Item(string id, PeerBase owner) {
            Owner = owner;
            Id = id;
        }

        ~Item() {
            Dispose(false);
        }

        // base properties
        public string Id { get; }
        public PeerBase Owner { get; }
        public int LocationId { get; private set; } = -1;
        public bool Transitive { get; set; }
        
        // to be classified...
        public bool Disposed { get; private set; }
        public bool Destroyed { get; private set; }

        // state properties, component specific
        public Vector2 PositionInLocation { get; private set; }
        public float MovementSpeedUnitsPerSecond { get; private set; } = 2f;
        
        public void ChangeLocation(int newLocationId) {
            if (newLocationId < 0) {
                throw new Exception("newLocationId can't be negative integer");
            }
            
            LocationId = newLocationId;
            Transitive = false;
            locationChangedChannel.Publish(new ItemLocationChangedMessage(newLocationId));
        }

        public IDisposable SubscribeLocationChanged(IFiber fiber, Action<ItemLocationChangedMessage> onLocChanged) {
            return locationChangedChannel.Subscribe(fiber, onLocChanged);
        }
        
        public void ChangePositionInLocation(Vector2 vector2) {
            PositionInLocation = vector2;
        }

        // todo implement destruction state script
        public void Destroy() {
            Destroyed = true;
        }

        public void Dispose() {
            logger.Info($"item {Id} is going to dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IDisposable SubscribeOnDispose(IFiber fiber, Action<ItemDisposedMessage> onDisposed) {
            return disposeChannel.Subscribe(fiber, onDisposed);
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private void Dispose(bool disposing) {
            if (disposing) {
                Disposed = true;
                disposeChannel.Publish(new ItemDisposedMessage(this));
            }
        }
    }
}
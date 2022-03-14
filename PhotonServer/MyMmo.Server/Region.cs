using System;
using System.Diagnostics.CodeAnalysis;
using ExitGames.Concurrency.Channels;
using ExitGames.Concurrency.Fibers;

namespace MyMmo.Server {
    [SuppressMessage("ReSharper", "ConvertToAutoProperty")]
    public class Region /*todo Dispose*/ {

        private readonly int id;

        private readonly Channel<RequestItemSnapshotMessage> requestItemSnapshotChannel =
            new Channel<RequestItemSnapshotMessage>();
        
        private readonly Channel<ItemRegionChangedMessage> itemRegionChangedChannel =
            new Channel<ItemRegionChangedMessage>();
        
        private readonly Channel<ItemEventMessage> regionalItemEventChannel =
            new Channel<ItemEventMessage>();
        
        public Region(int id) {
            this.id = id;
        }

        public int Id => id;

        public IDisposable SubscribeRequestItemSnapshot(IFiber fiber, Action<RequestItemSnapshotMessage> onRequestItemSnapshot) {
            return requestItemSnapshotChannel.Subscribe(fiber, onRequestItemSnapshot);
        }

        // Action will be executed on fiber of subscribed item
        public void PublishRequestItemSnapshot(Action<Item.Snapshot> snapshotCallback) {
            requestItemSnapshotChannel.Publish(new RequestItemSnapshotMessage(snapshotCallback));
        }

        public void PublishItemRegionChanged(ItemRegionChangedMessage itemRegionChangedMessage) {
            itemRegionChangedChannel.Publish(itemRegionChangedMessage);
        }

        public IDisposable SubscribeItemRegionChanged(IFiber fiber, Action<ItemRegionChangedMessage> onItemRegionChanged) {
            return itemRegionChangedChannel.Subscribe(fiber, onItemRegionChanged);
        }

        public void PublishItemEvent(ItemEventMessage eventMessage) {
            regionalItemEventChannel.Publish(eventMessage);
        }

        public IDisposable SubscribeItemEvent(IFiber fiber, Action<ItemEventMessage> onItemEvent) {
            return regionalItemEventChannel.Subscribe(fiber, onItemEvent);
        }
    }
}
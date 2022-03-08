using System;

namespace MyMmo.Server {
    public class RequestItemSnapshotMessage {

        public Action<Item.Snapshot> SnapshotCallback { get; }

        public RequestItemSnapshotMessage(Action<Item.Snapshot> snapshotCallback) {
            SnapshotCallback = snapshotCallback;
        }
        
    }

    public class ItemRegionChangedMessage {

        public Item.Snapshot Who { get; }
        public Region From { get; }
        public Region To { get; }

        public ItemRegionChangedMessage(Region @from, Region to, Item.Snapshot who) {
            From = @from;
            To = to;
            Who = who;
        }

    }
}
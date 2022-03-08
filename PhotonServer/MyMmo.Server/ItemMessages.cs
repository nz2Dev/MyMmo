using Photon.SocketServer;

namespace MyMmo.Server {
    public class ItemEventMessage {

        public SendParameters SendParameters { get; }
        public EventData Event { get; }
        public Item Source { get; }

        public ItemEventMessage(EventData @event, Item source, SendParameters sendParameters) {
            Event = @event;
            Source = source;
            SendParameters = sendParameters;
        }
    }

    public class ItemLocationChangedMessage {

        public int LocationId { get; }

        public ItemLocationChangedMessage(int locationId) {
            LocationId = locationId;
        }

    }

    public class ItemDisposedMessage {

        public Item Source { get; }

        public ItemDisposedMessage(Item source) {
            Source = source;
        }

    }
}
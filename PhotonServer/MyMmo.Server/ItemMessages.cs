namespace MyMmo.Server {
    
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
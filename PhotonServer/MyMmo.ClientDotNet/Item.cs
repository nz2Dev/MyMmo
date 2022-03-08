namespace MyMmo.Client {
    public class Item {

        public string Id { get; }
        public int LocationId { get; private set; }

        public Item(string id, int locationId) {
            Id = id;
            LocationId = locationId;
        }

        public void ChangeLocationId(int locationId) {
            LocationId = locationId;
        }

    }
}
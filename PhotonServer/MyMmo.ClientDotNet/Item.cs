using MyMmo.Commons.Primitives;

namespace MyMmo.Client {
    public class Item {

        public string Id { get; }
        public int LocationId { get; set; }
        public Vector2 PositionInLocation { get; set; }
        
        public bool IsDestroyed { get; set; }

        public Item(string id, int locationId, Vector2 positionInLocation) {
            Id = id;
            LocationId = locationId;
            PositionInLocation = positionInLocation;
        }

    }
}
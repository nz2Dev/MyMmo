using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Producers {
    public class ChangeLocationProducer {

        public Item Item { get; set; }
        public int NewLocationId { get; set; }

        public ChangeLocationProducer(Item item, int newLocationId) {
            Item = item;
            NewLocationId = newLocationId;
        }

        public ChangeLocationScript ProduceImmediatelyForEntireInterval() {
            return new ChangeLocationScript(
                Item.Id,
                Item.LocationId,
                NewLocationId
            );
        }

    }
}
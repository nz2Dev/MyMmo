using MyMmo.Server.Scripts;

namespace MyMmo.Server.Producers {
    public class ChangeLocationProducer : IScriptProducer<ChangeLocationScript> {

        private string itemId;
        private int newLocationId;

        public ChangeLocationProducer(string itemId, int newLocationId) {
            this.itemId = itemId;
            this.newLocationId = newLocationId;
        }

        public ChangeLocationScript ProduceImmediately(World world) {
            var item = world.GetItem(itemId);
            return new ChangeLocationScript(
                item.Id,
                item.LocationId,
                newLocationId
            );
        }

    }
}
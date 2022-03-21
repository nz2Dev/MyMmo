using System;
using MyMmo.Server.Scripts;

namespace MyMmo.Server.Producers {
    public class ChangeLocationProducer : IScriptProducer<ChangeLocationScript> {

        private World world;
        private string itemId;
        private int newLocationId;

        public ChangeLocationProducer(string itemId, int newLocationId, World world) {
            this.itemId = itemId;
            this.newLocationId = newLocationId;
            this.world = world;
        }

        public ChangeLocationScript ProduceImmediately() {
            if (!world.TryGetItem(itemId, out var item)) {
                throw new Exception("item not found: " + itemId);
            }
            
            return new ChangeLocationScript(
                item.Id,
                item.LocationId,
                newLocationId
            );
        }

    }
}
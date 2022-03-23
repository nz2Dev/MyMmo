using MyMmo.Server.Scripts;

namespace MyMmo.Server.Producers {
    public class DestroyItemProducer : IScriptProducer<IScript> {

        private readonly string itemId;

        public DestroyItemProducer(string itemId) {
            this.itemId = itemId;
        }

        public IScript ProduceImmediately(World world) {
            return new DestroyItemScript(itemId);
        }

    }
}
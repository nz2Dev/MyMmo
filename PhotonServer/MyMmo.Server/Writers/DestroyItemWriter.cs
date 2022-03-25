using MyMmo.Server.Scripts;

namespace MyMmo.Server.Writers {
    public class DestroyItemWriter : IScriptWriter {

        private readonly string itemId;

        public DestroyItemWriter(string itemId) {
            this.itemId = itemId;
        }

        public IScript ProduceImmediately(World world) {
            return new DestroyItemScript(itemId);
        }

        public void Write(World world, ScriptsClip clip) {
            clip.SetItemScript(itemId, ProduceImmediately(world));
        }

    }
}
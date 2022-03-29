using MyMmo.Server.Game.Scripts;

namespace MyMmo.Server.Game.Writers {
    public class DestroyItemWriter : IScriptWriter {

        private readonly string itemId;

        public DestroyItemWriter(string itemId) {
            this.itemId = itemId;
        }

        public IScript ProduceImmediately(World world) {
            return new DestroyItemScript(itemId);
        }

        public void WriteUpdate(World world, LocationScriptsClip clip, float deltaTimeSec) {
            if (clip.TryGetLastItemScriptOf<DestroyItemScript>(itemId, out var lastScript)) {
                if (lastScript.ItemId == itemId) {
                    return;
                }
            }
            clip.AddItemScript(itemId, ProduceImmediately(world));
        }

    }
}
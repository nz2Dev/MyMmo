using System;
using MyMmo.Server.Game.Scripts;

namespace MyMmo.Server.Game.Writers {
    public class ChangeLocationWriter : IScriptWriter {

        private string itemId;
        private int newLocationId;

        public ChangeLocationWriter(string itemId, int newLocationId) {
            this.itemId = itemId;
            this.newLocationId = newLocationId;
        }

        public IScript ProduceImmediately(World world) {
            var item = world.GetItem(itemId);
            return new ChangeLocationScript(
                item.Id,
                item.LocationId,
                newLocationId
            );
        }

        public void WriteUpdate(World world, LocationScriptsClip clip, float deltaTimeSec) {
            throw new NotImplementedException();
        }
    }
}
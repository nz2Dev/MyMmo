using System;
using MyMmo.Processing;

namespace MyMmo.Server.Game.Updates {
    public class ChangeLocationUpdate : BaseServerUpdate {

        private string itemId;
        private int newLocationId;

        public ChangeLocationUpdate(string itemId, int newLocationId) {
            this.itemId = itemId;
            this.newLocationId = newLocationId;
        }

        public override void Process(Scene scene) {
            throw new NotImplementedException();
        }

    }
}
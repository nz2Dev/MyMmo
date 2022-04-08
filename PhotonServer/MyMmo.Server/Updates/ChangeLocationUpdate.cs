using System;
using MyMmo.Processing;

namespace MyMmo.Server.Updates {
    public class ChangeLocationUpdate : BaseServerUpdate {

        private string itemId;
        private int newLocationId;

        public ChangeLocationUpdate(string itemId, int newLocationId) {
            this.itemId = itemId;
            this.newLocationId = newLocationId;
        }

        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            throw new NotImplementedException();
        }

    }
}
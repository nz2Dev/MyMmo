using System;

namespace MyMmo.Server.Game {
    public class ItemNotFound : Exception {

        public ItemNotFound(string itemId) :
            base($"Item with id {itemId} not found") {
        }

    }
}
using System;

namespace MyMmo.Server {
    public class ItemNotFound : Exception {

        public ItemNotFound(string itemId, World world) :
            base($"Item with id {itemId} not found in the world {world}") {
        }

    }
}
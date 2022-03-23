using System;

namespace MyMmo.Server {
    public class ItemNotFound : Exception {

        public ItemNotFound(string itemId) :
            base($"Item with id {itemId} not found") {
        }

    }
}
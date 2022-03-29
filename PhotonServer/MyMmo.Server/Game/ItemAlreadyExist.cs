using System;

namespace MyMmo.Server.Game {
    public class ItemAlreadyExist : Exception {

        public ItemAlreadyExist(Item item) : base($"Item with {item.Id} already exist") {
        }

    }
}
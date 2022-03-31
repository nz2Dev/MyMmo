using System;

namespace MyMmo.Server.Domain {
    public class ItemAlreadyExist : Exception {

        public ItemAlreadyExist(Item item) : base($"Item with {item.Id} already exist") {
        }

    }
}
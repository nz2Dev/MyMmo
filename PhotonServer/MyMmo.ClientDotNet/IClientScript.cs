using System.Collections.Generic;

namespace MyMmo.Client {
    public interface IClientScript {

        void ApplyClientState(Dictionary<string, Item> itemCache);

    }
}
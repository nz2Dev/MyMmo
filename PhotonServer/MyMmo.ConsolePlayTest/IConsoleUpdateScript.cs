using System.Collections.Generic;

namespace MyMmo.ConsolePlayTest {
    public interface IClientScript {

        void ApplyClientState(Dictionary<string, PlayTestItem> itemCache);

    }
}
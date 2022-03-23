using System.Collections.Generic;

namespace MyMmo.ConsolePlayTest {
    public interface IConsoleUpdateScript {

        void ApplyClientState(Dictionary<string, PlayTestItem> itemCache);

    }
}
using System.Collections.Generic;

namespace MyMmo.ConsolePlayTest {
    public interface IConsoleScriptApplier {

        void ApplyClientState(int locationId, Dictionary<string, ConsoleItem> itemCache);

    }
}
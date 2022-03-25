using System.Collections.Generic;

namespace MyMmo.ConsolePlayTest {
    public interface IConsoleScriptApplier {

        void ApplyClientState(Dictionary<string, ConsoleItem> itemCache);

    }
}
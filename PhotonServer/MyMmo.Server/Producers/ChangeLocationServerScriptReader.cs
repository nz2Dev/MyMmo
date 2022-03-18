using ExitGames.Logging;
using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Producers {
    public class ChangeLocationServerScriptReader {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly World world;

        public ChangeLocationServerScriptReader(World world) {
            this.world = world;
        }

        public void ApplyStateFromScript(ChangeLocationScript script) {
            if (!world.TryGetItem(script.ItemId, out var item)) {
                logger.Error($"item from script not found: " + script.ItemId);
                return;
            }         
            
            item.ChangeLocation(script.ToLocation);
        }

    }
}
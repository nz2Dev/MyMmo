using MyMmo.Commons.Scripts;

namespace MyMmo.Server.Game {
    public interface IScript {

        BaseScriptData ToScriptData();
        
        void ApplyState(World world);

    }
}
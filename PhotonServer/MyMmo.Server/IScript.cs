using MyMmo.Commons.Scripts;

namespace MyMmo.Server {
    public interface IScript {

        BaseScriptData ToScriptData();
        
        void ApplyState(World world);

    }
}
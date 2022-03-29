namespace MyMmo.Server.Game {
    public interface IScriptWriter {
        
        IScript ProduceImmediately(World world);
        
        void WriteUpdate(World world, LocationScriptsClip clip, float deltaTimeSec);

    }
}
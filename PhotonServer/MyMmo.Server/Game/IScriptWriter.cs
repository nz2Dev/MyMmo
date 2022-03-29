namespace MyMmo.Server.Game {
    public interface IScriptWriter {
        
        IScript ProduceImmediately(World world);
        
        void Write(World world, LocationScriptsClip clip);

    }
}
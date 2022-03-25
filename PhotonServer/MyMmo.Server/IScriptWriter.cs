namespace MyMmo.Server {
    public interface IScriptWriter {
        
        IScript ProduceImmediately(World world);
        
        void Write(World world, ScriptsClip clip);

    }
}
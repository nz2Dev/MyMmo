namespace MyMmo.Server {
    public interface IScriptProducer<out T> where T : IScript {

        T ProduceImmediately(World world);

    }
}
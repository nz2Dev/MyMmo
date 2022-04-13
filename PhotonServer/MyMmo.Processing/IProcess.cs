namespace MyMmo.Processing {
    public interface IProcess {

        bool Process(Scene scene, ProcessTimeContext timeContext);

    }
}
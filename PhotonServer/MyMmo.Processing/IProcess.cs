namespace MyMmo.Processing {
    public interface IProcess {

        bool Process(Scene scene, float timePassed, float timeLimit);

    }
}
namespace MyMmo.Processing {
    public interface IUpdate {

        bool Process(Scene scene, float timePassed, float timeLimit);

    }
}
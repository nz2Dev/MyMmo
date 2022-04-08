using MyMmo.Processing;
using MyMmo.Server.Domain;

namespace MyMmo.Server.Updates {
    public abstract class BaseServerUpdate : IUpdate {

        protected World world;

        internal void Attach(World worldInstance) {
            world = worldInstance;
        }
        
        public abstract bool Process(Scene scene, float timePassed, float timeLimit);

    }
}
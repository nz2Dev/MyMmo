using MyMmo.Processing;
using MyMmo.Server.Domain;

namespace MyMmo.Server.Updates {
    public abstract class BaseServerUpdate : IUpdate {

        protected World world;

        internal void Attach(World worldInstance) {
            world = worldInstance;
        }
        
        public abstract void Process(Scene scene);

    }
}
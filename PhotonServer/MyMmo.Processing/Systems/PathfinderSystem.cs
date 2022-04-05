namespace MyMmo.Processing.Systems {
    public class PathfinderSystem {

        public void Update(Scene scene, Entity entity) {
            if (entity.Pathfinder.Target != default) {
                var seekForce = entity.Pathfinder.SeekWithArrival(entity.Transform.Position, entity.Motion.Velocity);
                entity.Motion.ApplyForce(seekForce);
            }
        }

    }
}
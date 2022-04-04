namespace MyMmo.Processing.Systems {
    public class PathfinderSystem {

        public void Update(Scene scene, Entity entity) {
            if (entity.Pathfinder.Target != default) {
                var seekForce = entity.Motion.Seek(entity.Pathfinder.Target, entity.Transform.Position);
                entity.Motion.ApplyForce(seekForce);
            }
        }
        
    }
}
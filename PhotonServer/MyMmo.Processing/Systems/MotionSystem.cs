namespace MyMmo.Processing.Systems {
    public class MotionSystem {

        public void Update(Scene scene, Entity entity) {
            entity.Transform.SetPosition(entity.Transform.Position + entity.Motion.Velocity);
        }
        
    }
}
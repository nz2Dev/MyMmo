namespace MyMmo.Processing.Systems {
    public class MotionSystem {

        public void Update(Entity entity) {
            entity.Transform.Translate(entity.Motion.Velocity);
        }
        
    }
}
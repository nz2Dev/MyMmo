namespace MyMmo.Processing.Systems {
    public class MotionSystem {

        public void Update(Scene scene, Entity entity) {
            entity.Transform.Translate(entity.Motion.Velocity);
        }
        
    }
}
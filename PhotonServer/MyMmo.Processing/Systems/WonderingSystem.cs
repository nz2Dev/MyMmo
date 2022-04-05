using System;

namespace MyMmo.Processing.Systems {
    public class WonderingSystem {

        private readonly Random random = new Random();
        
        public void Update(Entity entity) {
            if (entity.Wondering.Enabled) {
                var wonderAngle = random.Next(-100, 100) * 0.01f;
                var wonderForce = entity.Wondering.Wonder(entity.Motion.Velocity, entity.Motion.Velocity, wonderAngle);
                entity.Motion.ApplyForce(wonderForce);
            }
        }
        
    }
}
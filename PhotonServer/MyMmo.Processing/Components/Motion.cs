using System.Numerics;
using MyMmo.Processing.Utils;

namespace MyMmo.Processing.Components {
    public class Motion {

        private const float MaxVelocity = 1f;
        private const float MaxSpeed = 2f;
        private const float MaxForce = 2f;
        
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        
        public void ApplyForce(Vector2 force) {
            Velocity += force;
            Velocity = VectorExtensions.Limit(Velocity, MaxVelocity);
        }
        
        public Vector2 Flee(Vector2 target, Vector2 source) {
            return Seek(target, source) * -1;
        }
        
        public Vector2 Seek(Vector2 target, Vector2 source) {
            var desire = target - source;
            desire = VectorExtensions.Limit(desire, MaxSpeed);
            var seek = desire - Velocity;
            return VectorExtensions.Limit(seek, MaxForce);
        }
        
    }
}
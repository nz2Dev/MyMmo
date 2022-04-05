using System.Numerics;
using MyMmo.Processing.Utils;

namespace MyMmo.Processing.Components {
    public class Motion {

        private const float MaxVelocity = 1f;
        
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        
        public void ApplyForce(Vector2 force) {
            Velocity += force;
            Velocity = VectorExtensions.Limit(Velocity, MaxVelocity);
        }
    }
}
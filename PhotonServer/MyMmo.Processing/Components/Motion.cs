using System;
using System.Numerics;
using MyMmo.Processing.Utils;

namespace MyMmo.Processing.Components {
    public class Motion {

        private const float MaxVelocity = 1f;
        private const float MaxSpeed = 2f;
        private const float MaxForce = 2f;
        private const float ArrivalActivationRadius = 1f;
        
        public Vector2 Velocity { get; private set; } = Vector2.Zero;
        
        public void ApplyForce(Vector2 force) {
            Velocity += force;
            Velocity = VectorExtensions.Limit(Velocity, MaxVelocity);
        }
        
        public Vector2 Flee(Vector2 target, Vector2 source) {
            return Seek(target, source) * -1;
        }

        public Vector2 SeekWithArrival(Vector2 target, Vector2 source) {
            return Seek(target, source, arrival: true);
        }
        
        public Vector2 Seek(Vector2 target, Vector2 source, bool arrival = false) {
            var desire = target - source;
            var distanceToTarget = desire.Length();
            var desireSpeed = MaxSpeed;
            if (arrival && distanceToTarget < ArrivalActivationRadius) {
                desireSpeed = NumberUtils.Map(distanceToTarget, 0, ArrivalActivationRadius, 0, MaxSpeed);
            }
            desire = VectorExtensions.SetMagnitude(desire, desireSpeed);
            var seek = desire - Velocity;
            return VectorExtensions.Limit(seek, MaxForce);
        }
        
    }
}
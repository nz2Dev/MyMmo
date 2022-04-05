using System.Numerics;
using MyMmo.Processing.Utils;

namespace MyMmo.Processing.Components {
    public class Pathfinder {

        private const float MaxSpeed = 2f;
        private const float MaxForce = 2f;
        private const float ArrivalActivationRadius = 1f;

        public Vector2 Target { get; set; }

        public Vector2 SeekWithArrival(Vector2 source, Vector2 sourceVelocity) {
            return Seek(source, sourceVelocity, arrival: true);
        }
        
        public Vector2 Seek(Vector2 source, Vector2 sourceVelocity, bool arrival = false) {
            var desire = Target - source;
            var distanceToTarget = desire.Length();
            var desireSpeed = MaxSpeed;
            if (arrival && distanceToTarget < ArrivalActivationRadius) {
                desireSpeed = NumberUtils.Map(distanceToTarget, 0, ArrivalActivationRadius, 0, MaxSpeed);
            }
            desire = VectorExtensions.SetMagnitude(desire, desireSpeed);
            var seek = desire - sourceVelocity;
            return VectorExtensions.Limit(seek, MaxForce);
        }
    }
}
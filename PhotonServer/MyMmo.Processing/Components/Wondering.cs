using System;
using System.Numerics;

namespace MyMmo.Processing.Components {
    public class Wondering {

        private const float WonderRadius = 50f;
        
        private float wonderAngle;
        
        public bool Enabled { get; set; }

        public Vector2 Wonder(Vector2 velocity, Vector2 heading, float angleDirection) {
            wonderAngle += angleDirection;
            
            var wonderDirection = new Vector2(
                (float) Math.Cos(wonderAngle) * WonderRadius,
                (float) Math.Sin(wonderAngle) * WonderRadius
            );
            
            var wonderDesire = heading * WonderRadius + wonderDirection;
            return wonderDesire - velocity;
        }
        
    }
}
using System;
using System.Numerics;

namespace MyMmo.Server.Primitives {
    public struct Line {

        public Vector2 pointA;
        public Vector2 pointB;

        public bool TryIntersect(Line other, out Vector2 intersectPoint) {
            // todo use math library for that
            throw new NotImplementedException();
        }
        
    }
}
using ProtoBuf;

namespace MyMmo.Commons.Primitives {
    
    [ProtoContract]
    public class Vector2 {

        [ProtoMember(1)]
        public float X { get; set; }
        [ProtoMember(2)]
        public float Y { get; set; }

        public override string ToString() {
            return GetType().Name + " { X = " + X + " Y = " + Y + " }";
        }

    }
}
using System;

namespace MyMmo.Playground {
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyKeyAttribute : Attribute {

        public byte Key;
        public bool IsOptional;

        public PropertyKeyAttribute() {
        }

        public PropertyKeyAttribute(byte key, bool optional) {
            Key = key;
            IsOptional = optional;
        }

    }
}
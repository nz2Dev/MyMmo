using System;
using System.Collections.Generic;

namespace MyMmo.Playground {
    public class EventDataConverter {

        public static T Convert<T>(Dictionary<byte, object> hashtable) where T : new() {
            var type = typeof(T);
            var output = new T();
            foreach (var propertyInfo in type.GetProperties()) {
                if (Attribute.GetCustomAttribute(propertyInfo, typeof(PropertyKeyAttribute)) is PropertyKeyAttribute attr) {
                    if (hashtable.TryGetValue(attr.Key, out var propertyValue)) {
                        propertyInfo.SetValue(output, propertyValue);
                    } else if (!attr.IsOptional) {
                        throw new PropertyValueNotFound(attr.Key, propertyInfo.Name);
                    }
                }
            }

            return output;
        }

        public static Dictionary<byte, object> ToDictionary(object paramsObject) {
            var paramsType = paramsObject.GetType();
            var dictionaryOut = new Dictionary<byte, object>();
            foreach (var propertyInfo in paramsType.GetProperties()) {
                if (Attribute.GetCustomAttribute(propertyInfo, typeof(PropertyKeyAttribute)) is PropertyKeyAttribute attr) {
                    var propertyValue = propertyInfo.GetValue(paramsObject);
                    if (propertyValue != null) {
                        dictionaryOut.Add(attr.Key, propertyValue);
                    } else if (!attr.IsOptional) {
                        throw new PropertyValueNotSet(attr.Key, propertyInfo.Name);
                    }
                }
            }

            return dictionaryOut;
        }

        public class PropertyValueNotFound : Exception {

            public PropertyValueNotFound(byte fieldKey, string propertyClassName) 
                : base($"fieldKey={fieldKey} propertyClassName={propertyClassName}") {
            }

        }

        public class PropertyValueNotSet : Exception {

            public PropertyValueNotSet(byte fieldKey, object paramsObject) 
                : base($"fieldKey={fieldKey} paramsObject={paramsObject}"){
            }

        }
    }
}
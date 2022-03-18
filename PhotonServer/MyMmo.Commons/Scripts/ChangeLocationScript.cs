using System;
using System.Collections.Generic;
using System.Text;

namespace MyMmo.Commons.Scripts {
    public class ChangeLocationScript {

        public string ItemId;
        public int FromLocation;
        public int ToLocation;

        public ChangeLocationScript(string itemId, int fromLocation, int toLocation) {
            ItemId = itemId;
            FromLocation = fromLocation;
            ToLocation = toLocation;
        }

        public static byte[] Serialize(object source) {
            var script = (ChangeLocationScript) source;
            List<byte> bytes = new List<byte>();
            bytes.AddRange(BitConverter.GetBytes(script.ItemId.Length));
            bytes.AddRange(Encoding.ASCII.GetBytes(script.ItemId));    
            bytes.AddRange(BitConverter.GetBytes(script.ToLocation));
            return bytes.ToArray();
        }

        public static ChangeLocationScript Deserialize(byte[] bytes) {
            var itemIdLength = BitConverter.ToInt32(bytes, 0);
            var itemId = Encoding.ASCII.GetString(bytes, 3, itemIdLength);
            var locationId = BitConverter.ToInt32(bytes, 3 + itemIdLength);
            return new ChangeLocationScript(itemId, -1, locationId);
        }
    }
}
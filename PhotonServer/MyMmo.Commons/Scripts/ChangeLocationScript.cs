using ProtoBuf;

namespace MyMmo.Commons.Scripts {
    
    [ProtoContract]
    public class ChangeLocationScript {

        [ProtoMember(1)]
        public string ItemId { get; set; }
        [ProtoMember(2)]
        public int FromLocation { get; set; }
        [ProtoMember(3)]
        public int ToLocation { get; set; }

        public ChangeLocationScript() {
        }

        public ChangeLocationScript(string itemId, int fromLocation, int toLocation) {
            ItemId = itemId;
            FromLocation = fromLocation;
            ToLocation = toLocation;
        }
    }
}
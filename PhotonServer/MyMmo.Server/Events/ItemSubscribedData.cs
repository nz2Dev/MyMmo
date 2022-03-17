using System.Diagnostics.CodeAnalysis;
using MyMmo.Commons;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ItemEnterData : DataContract {

        [DataMember(Code = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string ItemId { get; }

        [DataMember(Code = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; }

        public ItemEnterData(string itemId, int locationId) {
            LocationId = locationId;
            ItemId = itemId;
        }

        public static EventData CreateEventData(string itemId, int locationId) {
            return new EventData(
                (byte) EventCode.ItemSubscribedEvent,
                new ItemEnterData(itemId, locationId)
            );
        }
    }
}
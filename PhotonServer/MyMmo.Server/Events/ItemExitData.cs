using System.Diagnostics.CodeAnalysis;
using MyMmo.Commons;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ItemExitData {

        [DataMember(Code = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string ItemId { get; }

        public ItemExitData(string itemId) {
            ItemId = itemId;
        }

        public static EventData CreateEventData(string itemId) {
            return new EventData(
                (byte) EventCode.ItemExitEvent,
                new ItemExitData(itemId)
            );
        }
    }
}
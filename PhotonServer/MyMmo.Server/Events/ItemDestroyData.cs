using System.Diagnostics.CodeAnalysis;
using MyMmo.Commons;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ItemDestroyData : DataContract {

        [DataMember(Code = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string ItemId { get; }

        public ItemDestroyData(string itemId) {
            ItemId = itemId;
        }

    }
}
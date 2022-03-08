using MyMmo.Commons;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    public class ItemLocationChangedData : DataContract {

        [DataMember(Code = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string ItemId { get; set; }
        
        [DataMember(Code = ((byte) ParameterCode.LocationId), IsOptional = false)]
        public int LocationId { get; set; }

    }
}
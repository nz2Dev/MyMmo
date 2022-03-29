using MyMmo.Commons;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.App.Operations {
    public class EnterWorldResponseParams : DataContract {

        [DataMember(Code = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string AvatarItemId;
        
    }
}
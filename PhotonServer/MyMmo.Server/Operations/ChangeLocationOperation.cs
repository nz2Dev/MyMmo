using MyMmo.Commons;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Operations {
    public class ChangeLocationOperation : Operation {

        public ChangeLocationOperation(IRpcProtocol protocol, OperationRequest request)
            : base(protocol, request) 
        {
        }
        
        [DataMember(Code = (byte) ParameterCode.ItemId, IsOptional = false)]
        public string ItemId { get; set; }
        
        [DataMember(Code = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; set; }

    }
}
using MyMmo.Commons;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    public class RegionUpdatedData {

        [DataMember(Code = (byte) ParameterCode.SerializedScripts, IsOptional = false)]
        public byte[] Scripts { get; }
        
        [DataMember(Code = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; }

        public RegionUpdatedData(byte[] scripts, int locationId) {
            Scripts = scripts;
            LocationId = locationId;
        }

    }
}
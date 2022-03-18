using MyMmo.Commons;
using MyMmo.Commons.Scripts;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    public class RegionUpdatedData {

        [DataMember(Code = (byte) ParameterCode.ScriptsList, IsOptional = false)]
        public ChangeLocationScript[] Scripts { get; }
        
        [DataMember(Code = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; }

        public RegionUpdatedData(ChangeLocationScript[] scripts, int locationId) {
            Scripts = scripts;
            LocationId = locationId;
        }

    }
}
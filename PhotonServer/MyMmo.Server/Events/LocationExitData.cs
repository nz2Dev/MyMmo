using MyMmo.Commons;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    public class LocationExitData {

        [DataMember(Code = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; }

        public LocationExitData(int locationId) {
            LocationId = locationId;
        }
    }
}
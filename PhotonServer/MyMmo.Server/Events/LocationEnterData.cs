using MyMmo.Commons;
using MyMmo.Commons.Snapshots;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    
    /*todo remove DataContract inheritance for all events objects*/
    public class LocationEnterData {

        // todo check if it can be private
        [DataMember(Code = (byte) ParameterCode.SerializedLocationSnapshot, IsOptional = false)]
        public byte[] LocationSnapshotsBytes { get; }

        [DataMember(Code = (byte) ParameterCode.LocationId, IsOptional = false)]
        public int LocationId { get; }
        
        public LocationEnterData(int locationId, SceneSnapshotData sceneSnapshot) {
            LocationId = locationId;
            LocationSnapshotsBytes = SnapshotsDataProtocol.Serialize(sceneSnapshot);
        }
    }
}
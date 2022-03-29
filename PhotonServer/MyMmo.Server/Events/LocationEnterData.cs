using MyMmo.Commons;
using MyMmo.Commons.Snapshots;
using MyMmo.Server.Game;
using Photon.SocketServer.Rpc;

namespace MyMmo.Server.Events {
    
    /*todo remove DataContract inheritance for all events objects*/
    public class LocationEnterData {

        // todo check if it can be private
        [DataMember(Code = (byte) ParameterCode.SerializedLocationSnapshot, IsOptional = false)]
        public byte[] LocationSnapshotsBytes { get; }

        public LocationEnterData(LocationSnapshot locationSnapshot) {
            LocationSnapshotsBytes = SnapshotsDataProtocol.Serialize(locationSnapshot.ToData());
        }
    }
}
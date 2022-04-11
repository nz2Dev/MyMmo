using MyMmo.Commons;
using MyMmo.Commons.Snapshots;
using MyMmo.Server.Events;
using Photon.SocketServer;

namespace MyMmo.Server.Domain {
    public class ClientInterestArea : InterestArea {

        private readonly PeerBase peer;
        
        public ClientInterestArea(PeerBase peer, World world, string id) : base(world, id) {
            this.peer = peer;
        }

        protected override void OnLocationEnter(Location location, SceneSnapshotData sceneSnapshot) {
            peer.RequestFiber.Enqueue(() => {
                peer.SendEvent(
                    new EventData((byte) EventCode.LocationEnterEvent, new LocationEnterData(location.Id, sceneSnapshot)),
                    new SendParameters()
                );
            });
        }

        protected override void OnLocationEvent(LocationEventMessage message) {
            peer.RequestFiber.Enqueue(() => {
                peer.SendEvent(message.EventData, message.SendParameters);
            });
        }

        protected override void OnLocationExit(Location item) {
            peer.RequestFiber.Enqueue(() => {
                peer.SendEvent(
                    new EventData((byte) EventCode.LocationExitEvent, new LocationExitData(item.Id)),
                    new SendParameters()
                );
            });
        }

        protected override void OnDispose() {
        }

    }
}
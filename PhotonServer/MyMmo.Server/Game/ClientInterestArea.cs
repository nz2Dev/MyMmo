using System;
using System.Collections.Generic;
using MyMmo.Commons;
using MyMmo.Server.Events;
using Photon.SocketServer;

namespace MyMmo.Server.Game {
    public class ClientInterestArea : InterestArea {

        private readonly PeerBase peer;

        private readonly Dictionary<Location, IDisposable> locationEventSubscriptions =
            new Dictionary<Location, IDisposable>();

        public ClientInterestArea(PeerBase peer, World world, string id) : base(world, id) {
            this.peer = peer;
        }

        protected override void OnLocationEnter(LocationSnapshot snapshot) {
            peer.RequestFiber.Enqueue(() => {
                peer.SendEvent(
                    new EventData((byte) EventCode.LocationEnterEvent, new LocationEnterData(snapshot)),
                    new SendParameters()
                );

                locationEventSubscriptions.Add(snapshot.Source, snapshot.Source.SubscribeEvent(peer.RequestFiber, message => {
                    peer.SendEvent(message.EventData, message.SendParameters);
                }));
            });
        }

        protected override void OnLocationExit(Location item) {
            peer.RequestFiber.Enqueue(() => {
                if (locationEventSubscriptions.TryGetValue(item, out var subscription)) {
                    locationEventSubscriptions.Remove(item);
                    subscription?.Dispose();
                }

                peer.SendEvent(
                    new EventData((byte) EventCode.LocationExitEvent, new LocationExitData(item.Id)),
                    new SendParameters()
                );
            });
        }

        protected override void OnDispose() {
            foreach (var disposable in locationEventSubscriptions.Values) {
                disposable.Dispose();
            }

            locationEventSubscriptions.Clear();
        }

    }
}
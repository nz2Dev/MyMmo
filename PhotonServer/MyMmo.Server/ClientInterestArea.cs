using System;
using System.Collections.Generic;
using ExitGames.Logging;
using MyMmo.Server.Events;
using Photon.SocketServer;

namespace MyMmo.Server {
    public class ClientInterestArea : InterestArea {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        private readonly PeerBase peer;
        private readonly Dictionary<Region, IDisposable> regionEventSubscriptions = new Dictionary<Region, IDisposable>();
        
        public ClientInterestArea(PeerBase peer, World world, string id) : base(world, id) {
            this.peer = peer;
        }

        protected override void OnRegionEnter(Region region) {
            logger.Info($"client interest area {Id} subscribe to region {region.Id}' ItemEvent, so new event published on that region will be sent to peer {peer.ConnectionId}");
            regionEventSubscriptions.Add(region, region.SubscribeItemEvent(peer.RequestFiber, message => {
                logger.Info($"client interest area {Id} receive ItemEvent with eventCode {message.Event.Code} from region {region.Id}, sending event to peer {peer.ConnectionId}");
                peer.SendEvent(message.Event, message.SendParameters);
            }));
        }

        protected override void OnRegionExit(Region region) {
            logger.Info($"client interest area {Id} dispose subscription to region {region.Id}' ItemEvent, no events from that region will be sent to peer {peer.ConnectionId}");
            if (regionEventSubscriptions.TryGetValue(region, out var subscription)) {
                regionEventSubscriptions.Remove(region);
                subscription?.Dispose();
            }
        }

        protected override void OnItemEnter(Item.Snapshot snapshot) {
            peer.SendEvent(
                ItemEnterData.CreateEventData(snapshot.Id, snapshot.LocationId),
                new SendParameters()
            );
        }

        protected override void OnItemExit(Item item) {
            peer.SendEvent(
                ItemExitData.CreateEventData(item.Id),
                new SendParameters()
            );
        }

        protected override void OnDispose() {
            
        }

    }
}
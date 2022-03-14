using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using Photon.SocketServer.Concurrency;

namespace MyMmo.Server {
    public class InterestArea : IDisposable {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly object syncRoot = new object();

        private readonly string id;
        private readonly World world;

        private readonly HashSet<Region> enteredRegions = new HashSet<Region>();
        private readonly IDictionary<Region, IDisposable> regionSubscriptions = new Dictionary<Region, IDisposable>();
        private readonly IFiber subscriptionManagementFiber;

        private IDisposable followingSubscription;
        private Item followingItem;

        public InterestArea(World world, string id) {
            this.world = world;
            this.id = id;

            subscriptionManagementFiber = new PoolFiber();
            subscriptionManagementFiber.Start();
        }

        ~InterestArea() {
            Dispose(false);
        }

        public string Id => id;

        public void FollowLocationOf(Item item) {
            lock (syncRoot) {
                if (followingItem == null) {
                    followingItem = item;

                    WatchLocation(item.LocationId);

                    followingSubscription = new SubscriptionsCollection(
                        item.SubscribeLocationChanged(subscriptionManagementFiber, message => {
                            logger.Info($"interest area {id} receive LocationChanged {message.LocationId} from followed item, so it's going to follow that location");
                            WatchLocation(message.LocationId);
                        }),
                        item.SubscribeOnDispose(subscriptionManagementFiber, message => {
                            logger.Info($"interest area {id} receive ItemDisposed from followed item {message.Source.Id}, so it stops following");
                            StopFollowing();
                        })
                    );
                }
            }
        }

        public void StopFollowing() {
            lock (syncRoot) {
                followingSubscription?.Dispose();
                followingItem = null;
            }
        }

        private void WatchLocation(int locationId) {
            logger.Info($"interest area {id} starts watching surround regions of location {locationId} included");
            var regionsInFocus = world.GetSurroundedRegionsIncluded(locationId);
            var outOfFocusRegions = enteredRegions.Except(regionsInFocus).ToArray();
            UnsubscribeRegions(outOfFocusRegions);
            SubscribeRegions(regionsInFocus);
        }

        private void SubscribeRegions(IEnumerable<Region> regions) {
            foreach (var region in regions) {
                if (!enteredRegions.Contains(region)) {
                    enteredRegions.Add(region);
                    logger.Info($"interest area {id} is going to enter region {region.Id}");
                    EnterRegion(region);
                    logger.Info($"interest area {id} publish RequestItemSnapshot on region {region.Id} with enter callback");
                    region.PublishRequestItemSnapshot(snapshot => {
                        logger.Info($"interest area {id}' enter callback, item {snapshot.Id} send its snapshot from region {region.Id}");
                        OnItemEnter(snapshot);
                    });
                }
            }
        }

        private void UnsubscribeRegions(IEnumerable<Region> regions) {
            foreach (var region in regions) {
                enteredRegions.Remove(region);
                logger.Info($"interest area {id} is going to exit region {region.Id}");
                ExitRegion(region);
                logger.Info($"interest area {id} publish RequestItemSnapshot on region {region.Id} with exit callback");
                region.PublishRequestItemSnapshot(snapshot => {
                    logger.Info($"interest area {id}' exit callback, item {snapshot.Id} send its snapshot from region {region.Id}");
                    OnItemExit(snapshot.Source);
                });
            }
        }

        private void EnterRegion(Region region) {
            logger.Info($"interest area {id} subscribes to region {region.Id}'s ItemRegionChanged");
            regionSubscriptions[region] = region.SubscribeItemRegionChanged(subscriptionManagementFiber, message => {
                logger.Info($"interest area {id} receive ItemRegionChanged from region {region.Id}, item {message.Who.Id} changed its region {message.From?.Id} -> {message.To?.Id}");
                OnItemRegionChanged(message);
            });
            OnRegionEnter(region);
        }

        protected virtual void OnRegionEnter(Region region) {
        }

        private void ExitRegion(Region region) {
            logger.Info($"interest area {id} disposes subscription to region {region.Id}'s ItemRegionChangedMessage, and will not be notified of any item region changes of that region");
            if (regionSubscriptions.TryGetValue(region, out var subscription)) {
                regionSubscriptions.Remove(region);
                subscription.Dispose();
            }
            OnRegionExit(region);
        }

        protected virtual void OnRegionExit(Region region) {
        }

        private void OnItemRegionChanged(ItemRegionChangedMessage message) {
            var exitFromOurs = enteredRegions.Contains(message.From);
            var enterToOurs = enteredRegions.Contains(message.To);
            if (exitFromOurs && enterToOurs) {
                logger.Info($"interest area {id}, item {message.Who.Id} is going to move between our enteredRegions");
                // do nothing
            } else if (exitFromOurs) {
                logger.Info($"interest area {id}, item {message.Who.Id} is going to exit");
                OnItemExit(message.Who.Source);
            } else if (enterToOurs) {
                logger.Info($"interest area {id}, item {message.Who.Id} is going to enter");
                OnItemEnter(message.Who);
            }
        }

        protected virtual void OnItemEnter(Item.Snapshot snapshot) {
        }

        protected virtual void OnItemExit(Item item) {
        }

        public void Dispose() {
            logger.Info($"interest area {id} is going to dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose) {
            if (dispose) {
                foreach (var regionSubscription in regionSubscriptions.Values) {
                    regionSubscription.Dispose();
                }

                regionSubscriptions.Clear();
                enteredRegions.Clear();
                subscriptionManagementFiber.Dispose();
                followingSubscription?.Dispose();
                followingItem = null;

                OnDispose();
            }
        }

        protected virtual void OnDispose() {
        }

    }
}
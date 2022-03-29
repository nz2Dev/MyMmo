using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;

namespace MyMmo.Server.Game {
    public class InterestArea : IDisposable {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly object syncRoot = new object();

        private readonly string id;
        private readonly World world;

        private readonly HashSet<Location> enteredLocations = new HashSet<Location>();
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

        public void WatchLocationManually(int locationId, Action<LocationSnapshot> onLocationEnteredCallback) {
            subscriptionManagementFiber.Enqueue(() => {
                WatchLocation(locationId, onLocationEnteredCallback);
            });
        }

        private void WatchLocation(int locationId, Action<LocationSnapshot> onLocationEnteredCallback = null) {
            logger.Info($"interest area {id} starts watching surround locations of location {locationId} included");
            var locationsInFocus = world.GetSurroundedLocationsIncluded(locationId);
            var outOfFocusLocation = enteredLocations.Except(locationsInFocus).ToArray();
            UnsubscribeLocations(outOfFocusLocation);
            SubscribeLocations(locationsInFocus, onLocationEnteredCallback);
        }

        private void SubscribeLocations(IEnumerable<Location> locations, Action<LocationSnapshot> onLocationSnapshotCallback) {
            foreach (var location in locations) {
                if (!enteredLocations.Contains(location)) {
                    enteredLocations.Add(location);
                    logger.Info($"interest area {id} is going to enter location {location.Id}");
                    
                    logger.Info($"interest area {id} enqueue location {location.Id} snapshot callback");
                    location.EnqueueLocationSnapshot(snapshot => {
                        logger.Info($"interest area {id}' location {snapshot.Source.Id} callback with snapshot");
                        onLocationSnapshotCallback?.Invoke(snapshot);
                        OnLocationEnter(snapshot);
                    });
                }
            }
        }
        
        protected virtual void OnLocationEnter(LocationSnapshot snapshot) {
        }

        private void UnsubscribeLocations(IEnumerable<Location> locations) {
            foreach (var location in locations) {
                logger.Info($"interest area {id} is going to exit location {location.Id}");
                enteredLocations.Remove(location);

                logger.Info($"interest area {id}' location {location.Id} onLocationExit");
                OnLocationExit(location);
            }
        }

        protected virtual void OnLocationExit(Location item) {
        }

        public void Dispose() {
            logger.Info($"interest area {id} is going to dispose");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose) {
            if (dispose) {
                enteredLocations.Clear();
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
using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using MyMmo.Commons.Snapshots;

namespace MyMmo.Server.Domain {
    public class InterestArea : IDisposable {

        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly object syncRoot = new object();

        private readonly string id;
        private readonly World world;

        private readonly IFiber subscriptionManagementFiber;
        private readonly HashSet<Location> enteredLocations = new HashSet<Location>();
        private readonly Dictionary<Location, IDisposable> locationEventSubscriptions =
            new Dictionary<Location, IDisposable>();

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

        public void WatchLocationManually(int locationId) {
            subscriptionManagementFiber.Enqueue(() => {
                WatchLocation(locationId);
            });
        }

        public void EnqueueInLocationChangingFiber(Action onAllActionsIsDone) {
            subscriptionManagementFiber.Enqueue(onAllActionsIsDone);
        }

        private void WatchLocation(int locationId) {
            logger.Info($"interest area {id} starts watching surround locations of location {locationId} included");
            var locationsInFocus = world.GetSurroundedLocationsIncluded(locationId);
            var outOfFocusLocation = enteredLocations.Except(locationsInFocus).ToArray();
            UnsubscribeLocations(outOfFocusLocation);
            SubscribeLocations(locationsInFocus);
        }

        private void SubscribeLocations(IEnumerable<Location> locations) {
            foreach (var location in locations) {
                if (!enteredLocations.Contains(location)) {
                    enteredLocations.Add(location);
                    logger.Info($"interest area {id} is going to enter location {location.Id}");
                    
                    logger.Info($"interest area {id} enqueue location {location.Id} snapshot callback");
                    location.EnqueueSceneSnapshot(snapshot => {
                        logger.Info($"interest area {id}' location {location.Id} callback with snapshot");
                        OnLocationEnter(location, snapshot);
                    });
                    
                    logger.Info($"interest area {id} subscribes location {location.Id} events");
                    locationEventSubscriptions.Add(location, location.SubscribeEvent(subscriptionManagementFiber, message => {
                        logger.Info($"interest area {id}' location {location.Id} callback with event");
                        OnLocationEvent(message);
                    }));   
                }
            }
        }

        protected virtual void OnLocationEnter(Location location, SceneSnapshotData sceneSnapshot) {
        }

        protected virtual void OnLocationEvent(LocationEventMessage message) {
        }

        private void UnsubscribeLocations(IEnumerable<Location> locations) {
            foreach (var location in locations) {
                logger.Info($"interest area {id} is going to exit location {location.Id}");
                enteredLocations.Remove(location);

                // NOTE: if location fiber will publish event on InterestArea's fiber during this execution
                // there is a chance that OnLocationEvent might be called with already exited location
                // otherwise, we have to enqueue to location's update fiber, and from there call Dispose and then OnExit
                if (locationEventSubscriptions.TryGetValue(location, out var subscription)) {
                    locationEventSubscriptions.Remove(location);
                    subscription?.Dispose();
                }

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
                foreach (var disposable in locationEventSubscriptions.Values) {
                    disposable.Dispose();
                }
                
                locationEventSubscriptions.Clear();
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
using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Concurrency.Channels;
using ExitGames.Concurrency.Fibers;
using MyMmo.Commons;
using MyMmo.Commons.Scripts;
using MyMmo.Server.Events;
using MyMmo.Server.Producers;
using Photon.SocketServer;

namespace MyMmo.Server {
    public class Location : IDisposable {

        private const int UpdateInterval = 200;

        private readonly object requestLock = new object();

        private readonly World world;
        private readonly int id;

        private readonly IFiber updateFiber = new PoolFiber();
        private readonly List<IScriptProducer<IScript>> producers = new List<IScriptProducer<IScript>>();
        private bool scheduled;
        
        private readonly Channel<LocationEventMessage> locationEventChannel = 
            new Channel<LocationEventMessage>();

        public Location(World world, int id) {
            this.world = world;
            this.id = id;
            updateFiber.Start();
        }

        public int Id => id;

        public void EnqueueLocationSnapshot(Action<LocationSnapshot> snapshotCallback) {
            updateFiber.Enqueue(() => {
                var itemSnapshots = world.GetItemSnapshotsAtLocation(id);
                var locationSnapshot = new LocationSnapshot(this, itemSnapshots);
                snapshotCallback(locationSnapshot);
            });
        }

        public IDisposable SubscribeEvent(IFiber fiber, Action<LocationEventMessage> onLocationEventMessage) {
            return locationEventChannel.Subscribe(fiber, onLocationEventMessage);
        }
        
        public void RequestSpawnItem(SpawnClientAvatarProducer spawnClientAvatarProducer) {
            lock (requestLock) {
                CheckScheduling();
                producers.Add(spawnClientAvatarProducer);
            }
        }
        
        public void RequestChangeItemLocation(Item item, int newLocation) {
            lock (requestLock) {
                CheckScheduling();
                producers.Add(new ChangeLocationProducer(item.Id, newLocation, world));
            }
        }

        public void RequestMoveItemRandomly(Item item) {
            lock (requestLock) {
                CheckScheduling();
                producers.Add(new MoveItemRandomlyProducer(world, item.Id));
            }
        }

        private void CheckScheduling() {
            if (scheduled == false) {
                scheduled = true;
                updateFiber.Schedule(Update, UpdateInterval);
            }
        }

        private void Update() {
            var scripts = new List<IScript>();
            lock (requestLock) {
                scheduled = false;

                // first phase is to generate script
                foreach (var producer in producers) {
                    scripts.Add(producer.ProduceImmediately(world));
                }
                
                producers.Clear();
            }

            // then to apply state from them, so server will be the first one
            world.ExecuteStateScripts(scripts);
            
            // and send all the script data to everyone interested in them
            var scriptsData = scripts.Select(script => script.ToScriptData()).ToArray();
            var scriptsClip = ScriptsDataProtocol.Serialize(new ScriptsDataClip {ScriptsData = scriptsData});
            var regionUpdateData = new LocationUpdatedData(scriptsClip, id);
            var regionUpdateEvent = new EventData((byte) EventCode.RegionUpdated, regionUpdateData);
            locationEventChannel.Publish(new LocationEventMessage(regionUpdateEvent, new SendParameters()));
        }

        public void Dispose() {
            updateFiber.Dispose();
        }

    }
}
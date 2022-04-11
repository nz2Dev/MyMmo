using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Concurrency.Channels;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using MyMmo.Processing;
using MyMmo.Server.Events;
using Photon.SocketServer;

namespace MyMmo.Server.Domain {
    public class Location : IDisposable {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private const int UpdateInterval = 1000;

        private readonly object requestLock = new object();

        private readonly World world;
        private readonly int id;

        private readonly IFiber updateFiber = new PoolFiber();
        private readonly Scene scene;
        private bool scheduled;

        private readonly Channel<LocationEventMessage> locationEventChannel =
            new Channel<LocationEventMessage>();

        public Location(World world, int id, MapRegion mapRegion) {
            this.world = world;
            this.id = id;
            scene = new Scene(mapRegion: mapRegion);
            updateFiber.Start();
        }

        public int Id => id;
        public Scene Scene => scene;

        public void EnqueueSceneSnapshot(Action<SceneSnapshotData> snapshotCallback) {
            updateFiber.Enqueue(() => {
                snapshotCallback(scene.GenerateSnapshot());
            });
        }

        public IDisposable SubscribeEvent(IFiber fiber, Action<LocationEventMessage> onLocationEventMessage) {
            return locationEventChannel.Subscribe(fiber, onLocationEventMessage);
        }

        public void RequestUpdate(IUpdate update) {
            lock (requestLock) {
                scene.BufferUpdate(update);
                CheckScheduling();
            }
        }

        private void CheckScheduling() {
            if (!scheduled) {
                scheduled = true;
                updateFiber.Schedule(Update, UpdateInterval);
            }
        }

        private void Update() {
            logger.ConditionalDebug($"Location {id} start execution of Update");

            ScriptsClipData clipData;
            lock (requestLock) {
                clipData = scene.Simulate(0.2f, 10f);
                scheduled = false;
            }
            
            world.ApplyChanges(id, clipData);
            
            var scriptsClipBytes = ScriptsDataProtocol.Serialize(clipData);
            var regionUpdateData = new LocationUpdatedData(scriptsClipBytes, id);
            var regionUpdateEvent = new EventData((byte) EventCode.LocationUpdated, regionUpdateData);
            locationEventChannel.Publish(new LocationEventMessage(regionUpdateEvent, new SendParameters()));
        }
        
        public void Dispose() {
            updateFiber.Dispose();
        }

    }
}
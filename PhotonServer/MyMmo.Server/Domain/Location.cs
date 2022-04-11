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
using MyMmo.Server.Updates;
using Photon.SocketServer;

namespace MyMmo.Server.Domain {
    public class Location : IDisposable {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private const int UpdateInterval = 1000;

        private readonly object requestLock = new object();

        private readonly World world;
        private readonly int id;

        private readonly IFiber updateFiber = new PoolFiber();
        private readonly HashSet<BaseServerUpdate> updatesBuffer = new HashSet<BaseServerUpdate>();
        private readonly Scene locationScene = new Scene();
        private bool scheduled;

        private readonly Channel<LocationEventMessage> locationEventChannel =
            new Channel<LocationEventMessage>();

        public Location(World world, int id) {
            this.world = world;
            this.id = id;
            updateFiber.Start();
        }

        public int Id => id;

        public void EnqueueSceneSnapshot(Action<SceneSnapshotData> snapshotCallback) {
            updateFiber.Enqueue(() => {
                snapshotCallback(locationScene.GenerateSnapshot());
            });
        }

        public IDisposable SubscribeEvent(IFiber fiber, Action<LocationEventMessage> onLocationEventMessage) {
            return locationEventChannel.Subscribe(fiber, onLocationEventMessage);
        }

        public void RequestUpdate(BaseServerUpdate update) {
            lock (requestLock) {
                updatesBuffer.Add(update);
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

            List<BaseServerUpdate> updates;
            lock (requestLock) {
                updates = updatesBuffer.ToList();
                updatesBuffer.Clear();
                scheduled = false;
            }

            ScriptsClipData clipData;
            lock (World.SimulationSyncRoot) {
                updates.ForEach(update => update.Attach(world));
                clipData = locationScene.Simulate(updates, 0.2f, 10f);
            }
            
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
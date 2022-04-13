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

        private readonly int id;
        private readonly World world;
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IFiber updateFiber = new PoolFiber();
        private readonly List<IProcess> processBuffer = new List<IProcess>();
        private readonly Scene scene;
        
        private ScriptsClipData activeClip;
        private IDisposable updateScheduler;

        private readonly Channel<LocationEventMessage> locationEventChannel =
            new Channel<LocationEventMessage>();

        public Location(World world, int id, MapRegion mapRegion) {
            this.world = world;
            this.id = id;
            scene = new Scene(mapRegion: mapRegion);
            updateFiber.Start();
        }

        public int Id => id;

        public void EnqueueSceneSnapshot(Action<SceneSnapshotData> snapshotCallback) {
            updateFiber.Enqueue(() => {
                snapshotCallback(scene.GenerateSnapshot());
            });
        }

        public IDisposable SubscribeEvent(IFiber fiber, Action<LocationEventMessage> onLocationEventMessage) {
            return locationEventChannel.Subscribe(fiber, onLocationEventMessage);
        }

        private void PublishNextClip(ScriptsClipData clipData) {
            var scriptsClipBytes = ScriptsDataProtocol.Serialize(clipData);
            var regionUpdateData = new LocationUpdatedData(scriptsClipBytes, id);
            var regionUpdateEvent = new EventData((byte) EventCode.LocationUpdated, regionUpdateData);
            locationEventChannel.Publish(new LocationEventMessage(regionUpdateEvent, new SendParameters()));
        }

        public void RequestProcess(IProcess process) {
            updateFiber.Enqueue(() => {
                logger.ConditionalDebug($"location {id} receive process request: {process}");
                processBuffer.Add(process);
                TryScheduleNextUpdate();
            });
        }

        private void TryScheduleNextUpdate() {
            var timeToNextUpdateMillis = 0;
            var activeClipExpirationTime = activeClip?.EndTime() ?? world.Time;
            if (activeClipExpirationTime > world.Time) {
                var timeToActiveClipExpirationMillis = (activeClipExpirationTime - world.Time) * 1000;
                timeToNextUpdateMillis = (int) timeToActiveClipExpirationMillis;
            }
            
            updateScheduler?.Dispose();
            updateScheduler = updateFiber.Schedule(Update, timeToNextUpdateMillis);
        }

        private void Update() {
            logger.ConditionalDebug($"Location {id} start execution of Update");
            updateScheduler?.Dispose();
            updateScheduler = null;
            
            var processes = processBuffer.ToList();
            processBuffer.Clear();
                
            var clipData = scene.Simulate(processes, world.Time, 0.2f, 10f);
            activeClip = clipData;
            
            world.ApplyChanges(id, clipData);
            PublishNextClip(clipData);
        }

        public void Dispose() {
            updateScheduler?.Dispose(); // actually not necessary because updateFiber will be disposed next, but lets be specific 
            updateFiber.Dispose();
        }

    }
}
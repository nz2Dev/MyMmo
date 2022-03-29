using System;
using System.Collections.Generic;
using ExitGames.Concurrency.Channels;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Commons.Scripts;
using MyMmo.Server.Events;
using Photon.SocketServer;

namespace MyMmo.Server.Game {
    public class Location : IDisposable {

        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();
        
        private const int UpdateInterval = 1000;

        private readonly object requestLock = new object();

        private readonly World world;
        private readonly int id;

        private readonly IFiber updateFiber = new PoolFiber();
        private readonly HashSet<IScriptWriter> writers = new HashSet<IScriptWriter>();
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

        public void RequestProducer(IScriptWriter scriptWriter) {
            lock (requestLock) {
                CheckScheduling();
                if (writers.Contains(scriptWriter)) {
                    logger.Warn($"checking of scriptWriter {scriptWriter} containment in location {id}'s writers list return true, previous writer will be overwritten");
                }
                
                writers.Add(scriptWriter);
                logger.ConditionalDebug($"Location {id} received new producer {scriptWriter}, total amount is [{writers.Count}]");
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
            
            // first phase is to write scripts
            var clip = new LocationScriptsClip();
            lock (requestLock) {
                for (int i = 0; i < 5; i++) {
                    foreach (var writer in writers) {
                        writer.WriteUpdate(world, clip, 0.5f);
                    }    
                }
                
                scheduled = false;
                writers.Clear();
            }

            // then to apply state from them, so server will be the first one
            world.ExecuteStateScripts(clip);
            
            // and send all the script data to everyone interested in them
            var scriptsClipBytes = ScriptsDataProtocol.Serialize(clip.ToData());
            var regionUpdateData = new LocationUpdatedData(scriptsClipBytes, id);
            var regionUpdateEvent = new EventData((byte) EventCode.LocationUpdated, regionUpdateData);
            locationEventChannel.Publish(new LocationEventMessage(regionUpdateEvent, new SendParameters()));
        }

        public void Dispose() {
            updateFiber.Dispose();
        }
    }
}
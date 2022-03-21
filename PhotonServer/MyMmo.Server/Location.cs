using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Concurrency.Fibers;
using ExitGames.Logging;
using MyMmo.Commons;
using MyMmo.Commons.Scripts;
using MyMmo.Server.Events;
using MyMmo.Server.Producers;
using Photon.SocketServer;

namespace MyMmo.Server {
    public class Location : IDisposable {

        private const int UpdateInterval = 200;
        
        public int Id { get; }

        private readonly object requestLock = new object();
        private readonly ILogger logger = LogManager.GetCurrentClassLogger();
        private readonly IFiber fiber;
        private bool scheduled;
        private readonly List<ChangeLocationProducer> producers = new List<ChangeLocationProducer>();
        private readonly ChangeLocationServerScriptReader scriptReader;
        private readonly World world;

        public Location(World world, int id) {
            Id = id;
            
            fiber = new PoolFiber();
            fiber.Start();

            this.world = world;
            scriptReader = new ChangeLocationServerScriptReader(world);
        }

        public void RequestChangeItemLocation(Item item, int newLocation) {
            lock (requestLock) {
                if (scheduled == false) {
                    scheduled = true;
                    fiber.Schedule(Update, UpdateInterval);
                }

                var existed = producers.SingleOrDefault(producer => producer.Item.Id == item.Id);
                if (existed != null) {
                    producers.Remove(existed);
                }

                producers.Add(new ChangeLocationProducer(item, newLocation));
            }
        }

        private void Update() {
            lock (requestLock) {
                scheduled = false;

                // first phase is to generate script
                var scripts = new List<ChangeLocationScript>();
                foreach (var producer in producers) {
                    scripts.Add(producer.ProduceImmediatelyForEntireInterval());
                }
                
                // then to apply state from them, so server will be the first one
                // ...and yes, there will be multiple readers per type... for now only this one
                foreach (var script in scripts) {
                    scriptReader.ApplyStateFromScript(script);
                }
                
                // and send all the script to everyone interested in them.
                var region = world.GetRegion(Id);
                var serializedScripts = ScriptsDataProtocol.Serialize(new ScriptsClip {Scripts = scripts.ToArray()});
                logger.Info($"location serialized scripts clip object to bytes[{serializedScripts.Length}]: " + serializedScripts);
                var regionUpdateData = new RegionUpdatedData(serializedScripts, Id);
                var regionUpdateEvent = new EventData((byte) EventCode.RegionUpdated, regionUpdateData);
                region.PublishRegionEvent(new RegionEventMessage(regionUpdateEvent, new SendParameters()));
            }
        }

        public void Dispose() {
            fiber.Dispose();
        }

    }
}
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
    public class LocationSimulator : IDisposable {

        private const int UpdateInterval = 200;

        private readonly int locationId;
        private readonly object requestLock = new object();
        private readonly IFiber fiber;
        private readonly List<IScriptProducer<IScript>> producers = new List<IScriptProducer<IScript>>();
        private readonly World world;
        
        private bool scheduled;

        public LocationSimulator(World world, int locationId) {
            this.world = world;
            this.locationId = locationId;

            fiber = new PoolFiber();
            fiber.Start();
        }

        // todo should check for duplicates in producers list
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
                fiber.Schedule(Update, UpdateInterval);
            }
        }

        private void Update() {
            lock (requestLock) {
                scheduled = false;

                // first phase is to generate script
                var scripts = new List<IScript>();
                foreach (var producer in producers) {
                    scripts.Add(producer.ProduceImmediately());
                }
                producers.Clear();

                // then to apply state from them, so server will be the first one
                foreach (var script in scripts) {
                    script.ApplyState(world);
                }
                
                // and send all the script data to everyone interested in them
                var region = world.GetRegion(locationId);
                var scriptsData = scripts.Select(script => script.ToScriptData()).ToArray();
                var scriptsClip = ScriptsDataProtocol.Serialize(new ScriptsDataClip {ScriptsData = scriptsData});
                var regionUpdateData = new RegionUpdatedData(scriptsClip, locationId);
                var regionUpdateEvent = new EventData((byte) EventCode.RegionUpdated, regionUpdateData);
                region.PublishRegionEvent(new RegionEventMessage(regionUpdateEvent, new SendParameters()));
            }
        }

        public void Dispose() {
            fiber.Dispose();
        }

    }
}
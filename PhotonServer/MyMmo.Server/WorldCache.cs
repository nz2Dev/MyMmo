using System;
using System.Collections.Generic;
using System.Threading;
using ExitGames.Threading;

namespace MyMmo.Server {
    public class WorldCache : IDisposable {

        public static readonly WorldCache Instance = new WorldCache();

        private readonly Dictionary<string, World> worlds = new Dictionary<string, World>();

        // Used to synchronize access to the cache.
        private readonly ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();
        
        private WorldCache() {
        }

        ~WorldCache() {
            Dispose(false);
        }

        public bool TryCreate(string name, Func<World> factory) {
            using (WriteLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                if (worlds.TryGetValue(name, out var world)) {
                    return false;
                }

                world = factory();
                worlds.Add(name, world);
                return true;
            }
        }

        public bool TryGet(string name, out World world) {
            using (ReadLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                return worlds.TryGetValue(name, out world);
            }
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposing) {
                foreach (var world in worlds.Values) {
                    // todo dispose world.Dispose()
                }
            }
            
            readWriteLock.Dispose();
        }

    }
}
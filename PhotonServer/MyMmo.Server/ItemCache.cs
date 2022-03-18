using System.Collections.Generic;
using System.Threading;
using ExitGames.Threading;

namespace MyMmo.Server {
    public class ItemCache {
        
        private readonly Dictionary<string, Item> items = new Dictionary<string, Item>();

        // Used to synchronize access to the cache.
        private readonly ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();
        
        public bool TryAdd(Item item) {
            using (WriteLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                if (items.TryGetValue(item.Id, out var existed)) {
                    return false;
                }
                
                items.Add(item.Id, item);
                return true;
            }
        }

        public bool TryGetItem(string itemId, out Item item) {
            using (ReadLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                return items.TryGetValue(itemId, out item);
            }
        }
        
        public void Remove(Item item) {
            using (WriteLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                items.Remove(item.Id);
            }
        }

        public bool Contain(Item item) {
            using (ReadLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                return items.ContainsKey(item.Id);
            }
        }

    }
}
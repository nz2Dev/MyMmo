using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ExitGames.Threading;

namespace MyMmo.Server.Game {
    public class ItemCache {
        
        private readonly Dictionary<string, Item> items = new Dictionary<string, Item>();

        // Used to synchronize access to the cache.
        private readonly ReaderWriterLockSlim readWriteLock = new ReaderWriterLockSlim();

        public void Add(Item item) {
            if (!TryAdd(item)) {
                throw new ItemAlreadyExist(item);
            }
        }
        
        public bool TryAdd(Item item) {
            using (WriteLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                if (items.TryGetValue(item.Id, out var existed)) {
                    return false;
                }
                
                items.Add(item.Id, item);
                return true;
            }
        }

        public Item GetItem(string itemId) {
            if (!TryGetItem(itemId, out var item)) {
                throw new ItemNotFound(itemId);
            }

            return item;
        }
        
        public bool TryGetItem(string itemId, out Item item) {
            using (ReadLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                return items.TryGetValue(itemId, out item);
            }
        }

        public IEnumerable<Item> GetItemsWithLocationId(int locationId) {
            using (ReadLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                return items.Values.Where(item => item.LocationId == locationId);
            }
        }
        
        public void Remove(Item item) {
            using (WriteLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                items.Remove(item.Id);
            }
        }

        public bool Contain(string itemId) {
            using (ReadLock.TryEnter(readWriteLock, Settings.MaxLockWaitTimeMilliseconds)) {
                return items.ContainsKey(itemId);
            }
        }

    }
}
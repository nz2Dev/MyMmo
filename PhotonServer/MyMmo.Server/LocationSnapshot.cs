using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Snapshots;

namespace MyMmo.Server {
    public class LocationSnapshot {

        public Location Source { get; }
        private ICollection<ItemSnapshot> ItemSnapshots { get; }

        public LocationSnapshot(Location source, ICollection<ItemSnapshot> itemsSnapshot) {
            ItemSnapshots = itemsSnapshot;
            Source = source;
        }

        public LocationSnapshotData ToData() {
            return new LocationSnapshotData {
                ItemsSnapshotData = ItemSnapshots.Select(snapshot => snapshot.ToData()).ToArray(),
                LocationId = Source.Id
            };
        }
    }
}
using System.Collections.Generic;
using MyMmo.Commons.Snapshots;

namespace MyMmo.Client.Snapshots {
    public class LocationClientSnapshot {

        private LocationSnapshotData snapshotData;

        public LocationClientSnapshot(LocationSnapshotData snapshotData) {
            this.snapshotData = snapshotData;
        }

        public void SetState(Dictionary<string, Item> itemCache) {
            
        }

    }
}
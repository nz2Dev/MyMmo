using System.Collections.Generic;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using NUnit.Framework;

namespace TestProject1 {
    [TestFixture]
    public class Tests {

        [Test]
        public void Test1() {
            Assert.True(true);
            var bytes = ScriptsDataProtocol.Serialize(new ScriptsClipData {
                ScriptsData = new[] {
                    new ItemScriptsData {
                        ItemId = "item1",
                        ItemScriptData = new ChangeLocationScriptData {
                            ItemId = "it1",
                            FromLocation = 1,
                            ToLocation = 2
                        }    
                    }, 
                    new ItemScriptsData {
                        ItemId = "item2"
                    }
                }
            });

            var scriptsClip = ScriptsDataProtocol.Deserialize(bytes);
            Assert.IsInstanceOf<ChangeLocationScriptData>(scriptsClip.ScriptsData[0].ItemScriptData);
            Assert.AreEqual(((ChangeLocationScriptData) scriptsClip.ScriptsData[0].ItemScriptData).ItemId, "it1");
        }

        [Test]
        public void SnapshotsTest() {
            var bytes = SnapshotsDataProtocol.Serialize(new LocationSnapshotData {
                ItemsSnapshotData = new List<ItemSnapshotData> {
                    new ItemSnapshotData {
                        ItemId = "id_1"
                    }
                }.ToArray(),
                LocationId = 1
            });

            var data = SnapshotsDataProtocol.Deserialize(bytes);
            Assert.IsNotNull(data.ItemsSnapshotData);
            Assert.AreEqual(data.ItemsSnapshotData.Length, 1);
            Assert.AreEqual(data.ItemsSnapshotData[0].ItemId, "id_1");
        }

    }
}
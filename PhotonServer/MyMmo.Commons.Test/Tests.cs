using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                ItemDataArray = new[] {
                    new ItemScriptsData {
                        ItemId = "item1",
                        ScriptDataArray = new BaseScriptData[] {
                            new ChangeLocationScriptData {
                                ItemId = "it1",
                                FromLocation = 1,
                                ToLocation = 2
                            }
                        }    
                    }, 
                    new ItemScriptsData {
                        ItemId = "item2"
                    }
                }
            });

            var scriptsClip = ScriptsDataProtocol.Deserialize(bytes);
            Assert.IsInstanceOf<ChangeLocationScriptData>(scriptsClip.ItemDataArray[0].ScriptDataArray[0]);
            Assert.AreEqual(((ChangeLocationScriptData) scriptsClip.ItemDataArray[0].ScriptDataArray[0]).ItemId, "it1");
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

        [Test]
        public void YieldTest() {
            Console.WriteLine(ParallelYield().AsParallel().Aggregate((prev, curr) => prev + ", " + curr));
        }

        private IEnumerable<string> ParallelYield() {
            for (var a = 0; a < 2; a++) {
                yield return "A" + a;
            }

            for (var b = 0; b < 2; b++) {
                yield return "B" + b;
            }
        }
    }
}
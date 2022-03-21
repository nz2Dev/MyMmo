using MyMmo.Commons.Scripts;
using NUnit.Framework;

namespace TestProject1 {
    [TestFixture]
    public class Tests {

        [Test]
        public void Test1() {
            Assert.True(true);
            var bytes = ScriptsDataProtocol.Serialize(new ScriptsDataClip {
                ScriptsData = new BaseScriptData[] {
                    new ChangeLocationScriptData {
                        ItemId = "it1", 
                        FromLocation = 1,
                        ToLocation = 2
                    },
                    new ChangePositionScriptData {
                    }
                }
            });
            
            var scriptsClip = ScriptsDataProtocol.Deserialize(bytes);
            Assert.IsInstanceOf<ChangeLocationScriptData>(scriptsClip.ScriptsData[0]);
            Assert.AreEqual(((ChangeLocationScriptData) scriptsClip.ScriptsData[0]).ItemId, "it1");
            
        }

    }
}
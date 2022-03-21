using System.Collections.Generic;
using MyMmo.Commons.Scripts;
using NUnit.Framework;

namespace TestProject1 {
    [TestFixture]
    public class Tests {

        [Test]
        public void Test1() {
            Assert.True(true);
            var bytes = ScriptsDataProtocol.Serialize(new ScriptsClip {Scripts = new[] {new ChangeLocationScript("it1", 1, 2)}});
            var scriptsClip = ScriptsDataProtocol.Deserialize(bytes);
            Assert.AreEqual(scriptsClip.Scripts[0].ItemId, "it1");
        }

    }
}
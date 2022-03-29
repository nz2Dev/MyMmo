using System;
using MyMmo.Commons.Scripts;
using MyMmo.Server.Game;
using MyMmo.Server.Game.Writers;
using NUnit.Framework;

namespace MyMmo.Server.Test {
    [TestFixture]
    public class Tests {

        [Test]
        public void Test1() {
            var world = World.CreateDefaultWorld();
            var item = new Item("item1", null);
            item.ChangeLocation(World.RootLocationId);
            world.RegisterItem(item);
            
            var clip = new LocationScriptsClip();
            var writer = new MoveItemRandomlyWriter("item1");
            for (int i = 0; i < 5; i++) {
                writer.WriteUpdate(world, clip, 0.2f);
            }

            var firstItemScriptData = clip.ToData().ItemDataArray[0].ScriptDataArray[0];
            Assert.IsInstanceOf<ChangePositionScriptData>(firstItemScriptData);
            Assert.AreEqual(((ChangePositionScriptData) firstItemScriptData).Duration, 0.2f);
            
            foreach (var scriptData in clip.ToData().ItemDataArray[0].ScriptDataArray) {
                var script = ((ChangePositionScriptData) scriptData);
                Console.WriteLine($"From {script.FromPosition} to {script.ToPosition}, Destination {script.Destination.X}, {script.Destination.Y}");
            }
        }

    }
}
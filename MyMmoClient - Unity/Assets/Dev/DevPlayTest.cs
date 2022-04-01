using System.Collections.Generic;
using MyMmo.Commons.Snapshots;
using MyMmo.Processing;
using MyMmo.Processing.Utils;
using UnityEngine;

namespace Dev {
    public class DevPlayTest : MonoBehaviour {

        public Location devLocation;
        public GameObject playerPrefab;
        public UnityScriptsClipPlayer changesPlayer;

        private void Start() {
            var itemSnapshotData = new ItemSnapshotData {
                ItemId = "devItemOne",
                LocationId = devLocation.Id,
                PositionInLocation = new MyMmo.Commons.Primitives.Vector2 {
                    X = Random.Range(-5, 5),
                    Y = Random.Range(-5, 5)
                }
            };

            devLocation.PlaceAvatar(playerPrefab, itemSnapshotData);

            var entities = new[] {
                new Entity(
                    "devItemOne",
                    new MyMmo.Processing.Components.Transform(
                        itemSnapshotData.PositionInLocation.ToComputeVector(),
                        devLocation.Id,
                        data => { }
                    )
                )
            };

            var scene = new Scene(entities, new List<IUpdate>(new[] {
                new DevTestUpdate()
            }));

            var changesData = scene.Simulate();
            changesPlayer.PlayClip(devLocation.Id, changesData);
        }

    }

    public class DevTestUpdate : IUpdate {

        public void Process(Scene scene) {
            // Readme: MyMmo.Processing.dll targets .Net Framework 4.X, but because it references only system assemblies from there, it's compatible with Unity, e.g all the namespaces exist in Unity
            // MyMmo.Processing.dll reference System.Numerics namespace from separate .dll that was taken from netstandart2.0 subset that the Unity uses
            // (e.g somewhere from folded C:/.../Editor/netstandart/2.0/extensions/...) when there is compatibility level set to netstandard2.0 in Unity. 
            // so that's why it works probably...
            
            scene.GetEntity("devItemOne").Movement.Target = new System.Numerics.Vector2(Random.Range(-5, 5), Random.Range(-5 ,5));
            
            // so to work properly in Unity with compatibility level set to netstandaed out of the box, without referencing manually Unity's dll
            // the project that is build, should have target framework set to netstandard
        }
    }
}
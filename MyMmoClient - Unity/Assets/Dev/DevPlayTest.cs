using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using MyMmo.Processing;
using MyMmo.Processing.Utils;
using UnityEngine;
using Transform = MyMmo.Processing.Components.Transform;
using Vector2 = MyMmo.Commons.Primitives.Vector2;

namespace Dev {
    public class DevPlayTest : MonoBehaviour {

        public Location devLocation;
        public GameObject playerPrefab;
        public UnityScriptsClipPlayer changesPlayer;

        private void Start() {
            var itemIds = new[] {"devItem1", "devItem2", "devItem3", "devItem4", "devItem5"};
            var snapshots = itemIds.Select(id => {
                return new ItemSnapshotData {
                    ItemId = id,
                    LocationId = devLocation.Id,
                    PositionInLocation = new Vector2 {
                        X = Random.Range(-5, 5),
                        Y = Random.Range(-5, 5)
                    }
                };
            }).ToArray();
            
            var entities = snapshots.Select(snapshotData => {
                return new Entity(
                    snapshotData.ItemId,
                    new Transform(
                        snapshotData.PositionInLocation.ToComputeVector(),
                        snapshotData.LocationId,
                        data => { }
                    )
                );
            });

            var devTestUpdates = new[] {
                new DevTestUpdate()
            };

            var scene = new Scene(entities, new List<IUpdate>(devTestUpdates));
            var changesData = scene.Simulate();

            PlayClip(snapshots, changesData);
        }

        private void PlayClip(ItemSnapshotData[] snapshots, ScriptsClipData clip) {
            foreach (var snapshotData in snapshots) {
                devLocation.PlaceAvatar(playerPrefab, snapshotData);
            }
            
            changesPlayer.PlayClip(devLocation.Id, clip, () => {
                PlayClip(snapshots, clip);
            });
        }

    }

    public class DevTestUpdate : IUpdate {

        public void Process(Scene scene) {
            foreach (var entity in scene.Entities) {
                entity.Pathfinder.Target = new System.Numerics.Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            }
        }

    }
}
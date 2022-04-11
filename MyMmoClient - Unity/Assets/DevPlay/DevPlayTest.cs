using System.Collections.Generic;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using MyMmo.Processing;
using MyMmo.Processing.Utils;
using Player;
using UnityEngine;
using Transform = MyMmo.Processing.Components.Transform;
using Vector2 = MyMmo.Commons.Primitives.Vector2;

namespace DevPlay {
    public class DevPlayTest : MonoBehaviour {

        public Location devLocation;
        public GameObject playerPrefab;
        public bool replay = true;
        
        private ScriptsClipData simulatedClip;
        private ItemSnapshotData[] snapshots;

        private void Start() {
            ReSimulate();   
            PlayClip();
        }

        [ContextMenu("ReSimulate")]
        private void ReSimulate() {
            var itemIds = new[] {"devItem1", "devItem2", "devItem3", "devItem4", "devItem5"};
            snapshots = itemIds.Select(id => {
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

            var devTestUpdates = new IUpdate[] {
                new EnableWandering(),
                new Waiter(-1f)
            };

            var scene = new Scene(entities);
            simulatedClip = scene.Simulate(new List<IUpdate>(devTestUpdates), 0.2f, 4f);
        }

        private void PlayClip() {
            foreach (var snapshotData in snapshots) {
                devLocation.PlaceAvatar(playerPrefab, snapshotData);
            }
            
            devLocation.PlayClipImmediately(simulatedClip, () => {
                if (replay) {
                    PlayClip();
                }
            });
        }

    }

    class Waiter : IUpdate {

        private readonly float time;

        public Waiter(float time) {
            this.time = time;
        }

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            return time > 0 && timePassed > time;
        }
    }

    class SetRandomMoveTargets : IUpdate {

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            foreach (var entity in scene.Entities) {
                entity.Pathfinder.Target = new System.Numerics.Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            }

            return true;
        }

    } 
    
    class EnableWandering : IUpdate {

        public bool Process(Scene scene, float timePassed, float timeLimit) {
            foreach (var entity in scene.Entities) {
                entity.Wondering.Enabled = true;
            }

            return true;
        }
    }
}
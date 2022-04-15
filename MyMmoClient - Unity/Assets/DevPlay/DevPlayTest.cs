using System;
using System.Linq;
using MyMmo.Commons.Scripts;
using MyMmo.Commons.Snapshots;
using MyMmo.Processing;
using MyMmo.Processing.Utils;
using Player;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;
using Transform = MyMmo.Processing.Components.Transform;
using Vector2 = MyMmo.Commons.Primitives.Vector2;

namespace DevPlay {
    public class DevPlayTest : MonoBehaviour {

        public bool replay = true;
        public UnityWorldPlayer worldPlayer;

        private ScriptsClipData simulatedClip;
        private EntitySnapshotData[] snapshots;

        private void Awake() {
            Assert.IsNotNull(worldPlayer);
        }

        private void Start() {
            ReSimulate();   
            PlayClip();
        }

        [ContextMenu("ReSimulate")]
        private void ReSimulate() {
            var itemIds = new[] {"devItem1", "devItem2", "devItem3", "devItem4", "devItem5"};
            snapshots = itemIds.Select(id => {
                return new EntitySnapshotData {
                    ItemId = id,
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
                        snapshotData.PositionInLocation.ToComputeVector()
                    )
                );
            });

            var updates = new IProcess[] {
                new Waiter(-1f),
                new EnableWandering()
            };
            var scene = new Scene(entities);
            simulatedClip = scene.Simulate(updates, 0f, 0.2f, 4f);
        }

        private void PlayClip() {
            worldPlayer.EnterLocation(0, new SceneSnapshotData {
                EntitiesSnapshotData = snapshots
            });
            
            worldPlayer.UpdateLocation(0, simulatedClip, () => {
                if (replay) {
                    PlayClip();
                }
            });
        }

    }

    class Waiter : IProcess {

        private readonly float time;

        public Waiter(float time) {
            this.time = time;
        }

        public bool Process(Scene scene, ProcessTimeContext timeContext) {
            return time > 0 && timeContext.TimePassed > time;
        }
    }

    class SetRandomMoveTargets : IProcess {

        public bool Process(Scene scene, ProcessTimeContext timeContext) {
            foreach (var entity in scene.Entities) {
                entity.Pathfinder.Target = new System.Numerics.Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            }

            return true;
        }

    } 
    
    class EnableWandering : IProcess {

        public bool Process(Scene scene, ProcessTimeContext timeContext) {
            foreach (var entity in scene.Entities) {
                entity.Wondering.Enabled = true;
            }

            return true;
        }
    }
}
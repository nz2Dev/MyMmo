using System;
using System.Numerics;
using MyMmo.Commons.Scripts;
using MyMmo.Processing.Utils;

namespace MyMmo.Processing.Components {
    public class Transform {

        private readonly Action<ChangePositionScriptData> onChangesRecorded;
        private Vector2 positionChanges;

        public Transform(Vector2 position, int locationId, Action<ChangePositionScriptData> onChangesRecorded) {
            this.onChangesRecorded = onChangesRecorded;
            Position = position;
            LocationId = locationId;
        }

        public Vector2 Position { get; private set; }
        public int LocationId { get; }

        public void Translate(Vector2 vectorUnscaled) {
            positionChanges = vectorUnscaled;
        }

        public void RecordChanges(string id, Clip clip, float deltaTime) {
            if (positionChanges != default) {
                var fromPosition = Position;
                var nextPosition = NextPositionScaled(deltaTime);
                
                var changePositionScriptData = new ChangePositionScriptData {
                    ItemId = id,
                    FromPosition = fromPosition.ToDataVector2(),
                    ToPosition = nextPosition.ToDataVector2()
                };
                
                clip.AddChangesScript(id, changePositionScriptData);
                onChangesRecorded(changePositionScriptData);
                positionChanges = default;
                Position = nextPosition;
            }
        }

        private Vector2 NextPositionScaled(float changesDeltaTime) {
            return Position + positionChanges * changesDeltaTime;
        }

    }
}
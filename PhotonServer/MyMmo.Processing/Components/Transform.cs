using System;
using System.Numerics;
using MyMmo.Commons.Scripts;
using MyMmo.Processing.Utils;

namespace MyMmo.Processing.Components {
    public class Transform {

        private Vector2 lastRecordedPosition;
        private readonly Action<ChangePositionScriptData> onChangesRecorded;
        
        public Transform(Vector2 position, int locationId, Action<ChangePositionScriptData> onChangesRecorded) {
            lastRecordedPosition = position;
            Position = position;
            LocationId = locationId;
            this.onChangesRecorded = onChangesRecorded;
        }

        public Vector2 Position { get; private set; }
        public int LocationId { get; }

        public void SetPosition(Vector2 newPosition) {
            Position = newPosition;
        }

        public void RecordChanges(string id, Clip clip) {
            if (Position != lastRecordedPosition) {
                lastRecordedPosition = Position;
                var changePositionScriptData = new ChangePositionScriptData {
                    FromPosition = lastRecordedPosition.ToDataVector2(),
                    ToPosition = Position.ToDataVector2()
                };
                clip.AddChangesScript(id, changePositionScriptData);
                onChangesRecorded(changePositionScriptData);
            }
        }
    }
}
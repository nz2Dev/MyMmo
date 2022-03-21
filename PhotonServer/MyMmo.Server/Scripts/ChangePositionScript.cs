using System.Numerics;
using MyMmo.Commons.Scripts;
using MyMmo.Server.Producers;

namespace MyMmo.Server.Scripts {
    public class ChangePositionScript : IScript {

        private string itemId;
        private Vector2 fromPosition;
        private Vector2 toPosition;

        public ChangePositionScript(string itemId, Vector2 fromPosition, Vector2 toPosition) {
            this.itemId = itemId;
            this.fromPosition = fromPosition;
            this.toPosition = toPosition;
        }

        public BaseScriptData ToScriptData() {
            return new ChangePositionScriptData {
                FromPosition = fromPosition.ToDataVector2(),
                ToPosition = toPosition.ToDataVector2(),
                ItemId = itemId
            };
        }

        public void ApplyState(World world) {
            if (!world.TryGetItem(itemId, out var item)) {
                item.ChangePositionInLocation(toPosition);
            }
        }

    }
}
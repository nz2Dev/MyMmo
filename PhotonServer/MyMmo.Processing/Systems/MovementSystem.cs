using System.Numerics;

namespace MyMmo.Processing.Systems {
    public class MovementSystem {

        public void Update(Scene scene, Entity entity) {
            var moveVector = entity.Movement.Target - entity.Transform.Position;
            var maxUnitsDelta = 0.4f;
            var resultMoveVector = maxUnitsDelta * Vector2.Normalize(moveVector);
            entity.Transform.SetPosition(entity.Transform.Position + resultMoveVector);
        }

    }
}
using System.Numerics;
using MyMmo.Processing;
using MyMmo.Processing.Components;
using MyMmo.Processing.Utils;
using MyMmo.Server.Domain;

namespace MyMmo.Server.Updates {
    public class EnterFromLocationUpdate : BaseServerUpdate {

        private readonly Item item;
        private int fromLocationId;
        private int toLocationId;

        public EnterFromLocationUpdate(Item item, int fromLocationId, int toLocationId) {
            this.item = item;
            this.fromLocationId = fromLocationId;
            this.toLocationId = toLocationId;
        }

        public override bool Process(Scene scene, float timePassed, float timeLimit) {
            item.ChangeLocation(toLocationId);
            var mapRegion = world.GetMapRegion(toLocationId);
            item.ChangePositionInLocation(mapRegion.GetEnterPointFrom(fromLocationId)); // left side

            scene.RecordEnterImmediately(new Entity(item.Id, new Transform(item.PositionInLocation, item.LocationId, changes => {
                item.ChangePositionInLocation(changes.ToPosition.ToComputeVector());
            })));
            
            return true;
        }

    }
}
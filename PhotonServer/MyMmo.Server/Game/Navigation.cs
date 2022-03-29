using System.Collections.Generic;
using MyMmo.Server.Game.Primitives;

namespace MyMmo.Server.Game {
    public static class Navigation {

        public static bool TryAvoid(IEnumerable<IAgent> agents, IAgent source, out Line newTrajectory) {
            var steeringTrajectory = source.Trajectory;
            foreach (var agent in agents) {
                if (agent.Trajectory.TryIntersect(steeringTrajectory, out var intersectPoint)) {
                    steeringTrajectory = new Line {
                        pointA = steeringTrajectory.pointA,
                        pointB = intersectPoint
                    };
                }    
            }

            var avoided = !steeringTrajectory.Equals(source.Trajectory);
            newTrajectory = avoided ? steeringTrajectory : default;

            return avoided;
        }


        public interface IAgent {
            Line Trajectory { get; }
            float Radius { get; }
        }
    }
}
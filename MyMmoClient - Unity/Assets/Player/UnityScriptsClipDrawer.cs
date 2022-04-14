using System.Collections.Generic;
using Shapes;
using UnityEngine;

namespace Player {
    public class UnityScriptsClipDrawer : AnnotationShapes {

        public bool drawInScene;
        
        private readonly Dictionary<string, PolylinePath> entityPaths = new Dictionary<string, PolylinePath>();

        public void Clear() {
            entityPaths.Clear();
        }

        public void AddMovePoint(string entityId, Vector2 point, bool activated) {
            if (!entityPaths.TryGetValue(entityId, out var path)) {
                path = new PolylinePath();
                entityPaths[entityId] = path;
            }
            path.AddPoint(point, activated ? 1f : 0.1f, Color.green);
        }

        public override void OnDrawAnnotations() {
            if (!Application.isPlaying && drawInScene) {
                Clear();
                AddMovePoint("", Vector2.left, true);
                AddMovePoint("", Vector2.left + Vector2.up, true);
                AddMovePoint("", Vector2.up, false);
            }
            
            Draw.ResetAllDrawStates();
            Draw.Matrix = transform.localToWorldMatrix;
            Draw.Rotate(Quaternion.LookRotation(Vector3.down, Vector3.forward));
            Draw.Translate(0, 0, -0.05f);
            Draw.ThicknessSpace = ThicknessSpace.Meters;
            
            foreach (var path in entityPaths.Values) {
                Draw.Polyline(path, PolylineJoins.Round);
            }
        }

    }
}
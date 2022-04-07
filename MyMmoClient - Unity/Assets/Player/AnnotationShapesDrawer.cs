using Shapes;
using UnityEngine;

namespace Player {
    
    [ExecuteInEditMode]
    public class AnnotationShapesDrawer : ImmediateModeShapeDrawer {

        public override void DrawShapes(Camera cam) {
            using (Draw.Command(cam)) {
                var avatars = FindObjectsOfType<AvatarItem>();
                foreach (var avatarItem in avatars) {
                    avatarItem.DrawShapesAnnotation();
                }   
            }
        }

    }
}
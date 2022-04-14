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

                var annotations = FindObjectsOfType<AnnotationShapes>();
                foreach (var annotationShapes in annotations) {
                    annotationShapes.OnDrawAnnotations();
                }
            }
        }

    }
}
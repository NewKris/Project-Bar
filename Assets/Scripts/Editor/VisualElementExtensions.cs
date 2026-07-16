using UnityEngine.UIElements;

namespace Editor {
    public static class VisualElementExtensions {
        public static void SetPadding(this VisualElement element, float top, float right, float bottom, float left) {
            element.style.paddingBottom = bottom;
            element.style.paddingLeft = left;
            element.style.paddingRight = right;
            element.style.paddingTop = top;
        }
    }
}
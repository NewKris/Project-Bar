using UnityEditor;
using UnityEngine.UIElements;

namespace Editor {
    public static class VisualElementExtensions {
        public static void SetPadding(this VisualElement element, float top, float right, float bottom, float left) {
            element.style.paddingBottom = bottom;
            element.style.paddingLeft = left;
            element.style.paddingRight = right;
            element.style.paddingTop = top;
        }
        
        public static void AddStyleClass(this VisualElement element, params string[] styles) {
            foreach (string style in styles)
                element.AddToClassList(style);
        }

        public static void LoadStyleSheet(this VisualElement element, params string[] styleSheets) {
            foreach (string styleSheetName in styleSheets) {
                StyleSheet styleSheet = EditorGUIUtility.Load(styleSheetName) as StyleSheet;
                element.styleSheets.Add(styleSheet);
            }
        }
    }
}
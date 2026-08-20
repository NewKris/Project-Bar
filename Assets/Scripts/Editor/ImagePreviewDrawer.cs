using Runtime.Utility;
using UnityEditor;
using UnityEngine;

namespace Editor {
    [CustomPropertyDrawer(typeof(ImagePreview))]
    public class ImagePreviewDrawer : PropertyDrawer {
        private const int PREVIEW_HEIGHT = 50;
        private const int SPACING = 5;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            Rect field = position;
            field.height = 20;
            EditorGUI.ObjectField(field, property, label);

            Rect preview = position;
            preview.y += PREVIEW_HEIGHT * 0.5f;
            preview.height = PREVIEW_HEIGHT;
            
            if (property.objectReferenceValue is Sprite sprite) {
                preview.width = CalculateWidth(preview.height, sprite.texture);
                GUI.DrawTexture(preview, sprite.texture);
            } else if (property.objectReferenceValue is Texture2D texture) {
                preview.width = CalculateWidth(preview.height, texture);
                GUI.DrawTexture(preview, texture);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float extraHeight = 0;
            
            if (property.objectReferenceValue is Sprite or Texture) {
                extraHeight = PREVIEW_HEIGHT + SPACING * 2;
            }

            return base.GetPropertyHeight(property, label) + extraHeight;
        }

        private float CalculateWidth(float height, Texture texture) {
            float ratio = texture.width / (float) texture.height;
            return ratio * height;
        }
    }
}
using System;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Editor {
    public static class VisualElementFactory {
        public static VisualElement CreateButton(string text, Action onClick) {
            return new Button(onClick) {
                text = text
            };
        }
        
        public static Button CreateButton(Texture icon, Action onClick) {
            return new Button(onClick) {
                iconImage = Background.FromTexture2D(icon as Texture2D)
            };
        }
        
        public static VisualElement CreateTextArea(string label, string value) {
            return new TextField(label) {
                value = value,
                multiline = true,
                style = { minHeight = 30 }
            };
        }
        
        public static VisualElement CreateTextField(string label, string value) {
            return new TextField(label) {
                value = value,
                multiline = false
            };
        }
        
        public static VisualElement CreateLabel(string text) {
            return new Label(text);
        } 
        
        public static VisualElement CreateObjectField<T>(string label, Object value) where T : Object {
            return new ObjectField(label) {
                allowSceneObjects = false,
                objectType = typeof(T),
                value = value
            };
        }
    }
}
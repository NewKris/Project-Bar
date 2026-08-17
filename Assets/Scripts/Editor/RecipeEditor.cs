using Runtime.Old_Systems.Drink;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
    [CustomEditor(typeof(Recipe))]
    public class RecipeEditor : UnityEditor.Editor {
        public override VisualElement CreateInspectorGUI() {
            Recipe recipe = (Recipe)target;
            VisualElement root = new VisualElement();
            root.LoadStyleSheet("recipe_style.uss");
            root.AddStyleClass("root");
            
            root.Add(DrawContentsFields(recipe));
            root.Add(DrawUiFields(recipe));
            
            return root;
        }

        private VisualElement DrawContentsFields(Recipe recipe) {
            VisualElement div = new VisualElement();
            div.AddStyleClass("box");
            div.Add(CreateBoxTitle("Ingredients"));
            
            div.Add(CreateObjectField("Drink Container", recipe.contents.drinkContainer));
            
            return div;
        }

        private VisualElement DrawUiFields(Recipe recipe) {
            VisualElement div = new VisualElement();
            div.AddStyleClass("box");
            div.Add(CreateBoxTitle("UI"));
            div.Add(CreateObjectField("Icon", recipe.icon));
            
            div.Add(CreateTextField("Display Name", recipe.customDisplayName, "Mojito"));
            
            return div;
        }

        private VisualElement CreateTextField(string label, string value, string placeHolder) {
            TextField textField = new TextField(label);
            textField.value = value;

            return textField;
        }

        private VisualElement CreateObjectField<T>(string label, T value) where T : Object {
            ObjectField field = new ObjectField(label);
            field.allowSceneObjects = false;
            field.objectType = typeof(T);
            field.value = value;

            return field;
        }
        
        private VisualElement CreateBoxTitle(string text) {
            Label label = new Label(text);
            label.AddStyleClass("box-title");
            return label;
        }
    }
}
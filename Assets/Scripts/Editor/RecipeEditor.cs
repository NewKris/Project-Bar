using Runtime.Drinks;
using Runtime.Old_Systems.Drink;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor {
    //[CustomEditor(typeof(Recipe))]
    public class RecipeEditor : UnityEditor.Editor {
        private static readonly string[] StyleSheets = new[] {
            "recipe_style.uss"
        };
        
        public override VisualElement CreateInspectorGUI() {
            Recipe recipe = (Recipe)target;
            VisualElement root = new VisualElement();
            root.LoadStyleSheet(StyleSheets);
            root.AddStyleClass("root");
            
            root.Add(DrawUiFields(recipe));
            root.Add(DrawContentsFields(recipe));
            
            return root;
        }

        private VisualElement DrawContentsFields(Recipe recipe) {
            VisualElement div = new VisualElement();
            div.AddStyleClass("box");
            
            div.Add(CreateBoxTitle("Ingredients"));
            div.Add(VisualElementFactory.CreateObjectField<DrinkContainer>("Drink Container", recipe.contents.drinkContainer));
            div.Add(CreateIngredientList(recipe));
            
            return div;
        }

        private VisualElement DrawUiFields(Recipe recipe) {
            VisualElement div = new VisualElement();
            div.AddStyleClass("box");
            
            div.Add(CreateBoxTitle("UI"));
            div.Add(VisualElementFactory.CreateObjectField<Sprite>("Icon", recipe.icon));
            div.Add(VisualElementFactory.CreateTextField("Display Name", recipe.customDisplayName));
            div.Add(VisualElementFactory.CreateTextArea("Description", recipe.description));
            
            return div;
        }

        private VisualElement CreateIngredientList(Recipe recipe) {
            VisualElement div = new VisualElement();
            div.AddStyleClass("ingredient-list");
            
            foreach (IngredientGroup group in recipe.contents.ingredientGroups) {
                div.Add(CreateIngredientGroup(group, recipe));
            }
            
            div.Add(CreateAddGroupButton(recipe));

            return div;
        }

        private VisualElement CreateIngredientGroup(IngredientGroup group, Recipe recipe) {
            VisualElement div = new VisualElement();

            div.AddStyleClass("ingredient-group");
            
            return div;
        }

        private VisualElement CreateAddGroupButton(Recipe recipe) {
            return VisualElementFactory.CreateButton(
                EditorGUIUtility.IconContent("Toolbar Plus").image,
                () => {
                    recipe.contents.ingredientGroups.Add(new IngredientGroup());
                    MarkDirty(recipe);
                    Repaint();
                }
            );
        }

        private void MarkDirty(Object obj) {
            EditorUtility.SetDirty(obj);
        }
        
        private VisualElement CreateBoxTitle(string text) {
            VisualElement label = VisualElementFactory.CreateLabel(text);
            label.AddStyleClass("box-title");
            return label;
        }
    }
}
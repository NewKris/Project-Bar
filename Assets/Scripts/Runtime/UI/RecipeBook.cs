using System;
using NaughtyAttributes;
using Runtime.Customers.Tutorial_Agent;
using Runtime.Drink;
using Runtime.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class RecipeBook : MonoBehaviour {
        public GameObject entryPrefab;
        public Recipe[] initialRecipes;
        public UnlockRecipesEventPort port;
        
        [Header("Formatting")]
        [TextArea] public string textFormat;
        public string ingredientSeparator = ", ";

        [Foldout("Keys")] public string indexKey = "{id}";
        [Foldout("Keys")] public string nameKey = "{name}";
        [Foldout("Keys")] public string descriptionKey = "{description}";
        [Foldout("Keys")] public string containerKey = "{container}";
        [Foldout("Keys")] public string recipeIngredientsKey = "{ingredients}";

        private int _recipeIdx;
        
        public void AddRecipes(Recipe[] newRecipes) {
            Transform entryParent = FindContentParent().transform;
            
            foreach (Recipe recipe in newRecipes) {
                PrintRecipe(recipe, entryParent);
            }
        }

        private void Awake() {
            port.onRecipesUnlocked += AddRecipes;
        }

        private void OnDestroy() {
            port.onRecipesUnlocked -= AddRecipes;
        }

        private void Start() {
            PrintInitialRecipes();
        }

        private void PrintInitialRecipes() {
            Transform entryParent = FindContentParent().transform;

            foreach (Recipe recipe in initialRecipes) {
                PrintRecipe(recipe, entryParent);
            }
        }

        private void PrintRecipe(Recipe recipe, Transform parent) {
            GameObject entry = Instantiate(entryPrefab, parent);
            SetEntryText(_recipeIdx, recipe, entry);
            SetEntrySprite(recipe, entry);
            _recipeIdx++;
        }

        private void SetEntrySprite(Recipe recipe, GameObject entry) {
            entry.GetComponentInChildren<Image>().sprite = recipe.icon;
        }

        private void SetEntryText(int index, Recipe recipe, GameObject entry) {
            TMP_Text recipeText = entry.GetComponentInChildren<TMP_Text>();
            
            recipeText.text = textFormat;
            recipeText.text = recipeText.text.Replace(indexKey, (index + 1).ToString());
            recipeText.text = recipeText.text.Replace(nameKey, recipe.DisplayName);
            recipeText.text = recipeText.text.Replace(descriptionKey, recipe.description);
            recipeText.text = recipeText.text.Replace(containerKey, recipe.contents.drinkContainer.DisplayName);
            
            string ingredients = "";
            foreach (Ingredient ingredient in recipe.contents.ingredients) {
                ingredients += ingredient.DisplayName + ingredientSeparator;
            }
            ingredients = ingredients.Substring(0, ingredients.Length - ingredientSeparator.Length);
            
            recipeText.text = recipeText.text.Replace(recipeIngredientsKey, ingredients);
        }

        private GameObject FindContentParent() {
            return GetComponentInChildren<LayoutGroup>().gameObject;
        }
    }
}

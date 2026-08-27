using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Runtime.Customers.Tutorial_Agent;
using Runtime.Drinks;
using TMPro;
using UnityEngine;

namespace Runtime.UI.NewRecipeBook {
    public class RecipeBookV2 : MonoBehaviour {
        [Required] public UnlockRecipesEventPort port;
        public Recipe[] initialRecipes;
        public int recipesPerPage;
        
        [Header("UI")]
        public TMP_Text pageNumberText;
        
        [Header("Prefab")]
        public GameObject recipeRowPrefab;
        public Transform recipeRowParent;
        
        [Header("Formatting")] 
        [TextArea] public string textFormat;
        public string ingredientSeparator = ", ";
        
        [Foldout("Keys")] public string indexKey = "{id}";
        [Foldout("Keys")] public string nameKey = "{name}";
        [Foldout("Keys")] public string descriptionKey = "{description}";
        [Foldout("Keys")] public string containerKey = "{container}";
        [Foldout("Keys")] public string recipeIngredientsKey = "{ingredients}";

        private int _currentPage;
        private RecipeSortMode _currentSortMode;
        private List<Recipe> _recipes;
        
        private int MaxPageIndex => Mathf.FloorToInt(_recipes.Count / (float)recipesPerPage);

        public void SortAlphabetically() {
            TryToggleSort(RecipeSortMode.ALPHABETICAL);
            SortList();
            DrawPage(_currentPage);
        }

        public void SortByAlcohol() {
            TryToggleSort(RecipeSortMode.ALCOHOLIC);
            SortList();
            DrawPage(_currentPage);
        }

        public void SortByContainer() {
            TryToggleSort(RecipeSortMode.CONTAINER);
            SortList();
            DrawPage(_currentPage);
        }
        
        public void ShowNext() {
            _currentPage = Mathf.Min(MaxPageIndex, _currentPage + 1);
            DrawPage(_currentPage);
        }
        
        public void ShowPrevious() {
            _currentPage = Mathf.Max(_currentPage - 1, 0);
            DrawPage(_currentPage);
        }

        private void Awake() {
            _currentSortMode = RecipeSortMode.CONTAINER;
            _currentPage = 0;
            
            _recipes = new List<Recipe>(20);
            _recipes.AddRange(initialRecipes);
            
            SortList();
            DrawPage(_currentPage);

            port.onRecipesUnlocked += AddRecipes;
        }

        private void OnDestroy() {
            port.onRecipesUnlocked -= AddRecipes;
        }

        private void AddRecipes(Recipe[] newRecipes) {
            foreach (Recipe recipe in newRecipes) {
                _recipes.Add(recipe);
            }
            
            SortList();
            DrawPage(_currentPage);
        }

        private void TryToggleSort(RecipeSortMode mode) {
            _currentSortMode = _currentSortMode == mode ? RecipeSortMode.CONTAINER : mode;
        }

        private void SortList() {
            switch (_currentSortMode) {
                case RecipeSortMode.ALPHABETICAL:
                    _recipes.Sort(Recipe.CompareName);
                    break;
                case RecipeSortMode.CONTAINER:
                    _recipes.Sort(Recipe.CompareContainer);
                    break;
                case RecipeSortMode.ALCOHOLIC:
                    _recipes.Sort(Recipe.CompareAlcohol);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawPage(int pageIndex) {
            foreach (Transform child in recipeRowParent) {
                Destroy(child.gameObject);
            }
            
            int startRecipe = pageIndex * recipesPerPage;
            int endRecipe = Mathf.Min(startRecipe + recipesPerPage, _recipes.Count);
            
            for (int i = startRecipe; i < endRecipe; i++) {
                PrintRecipe(i, _recipes[i]);
            }

            pageNumberText.text = $"{pageIndex + 1}/{MaxPageIndex + 1}";
        }

        private void PrintRecipe(int recipeIndex, Recipe recipe) {
            RecipeRow row = Instantiate(recipeRowPrefab, recipeRowParent).GetComponent<RecipeRow>();
            row.SetInfo(CreateRecipeText(recipeIndex, recipe), recipe.icon, recipe);
        }

        private string CreateRecipeText(int recipeIndex, Recipe recipe) {
            string recipeText = textFormat;
            
            recipeText = recipeText.Replace(indexKey, (recipeIndex + 1).ToString());
            recipeText = recipeText.Replace(nameKey, recipe.DisplayName);
            recipeText = recipeText.Replace(descriptionKey, recipe.description);
            recipeText = recipeText.Replace(containerKey, recipe.contents.drinkContainer.DisplayName);
            
            string ingredients = "";
            recipe.contents.ForEachIngredient(x => {
                ingredients += x.DisplayName + ingredientSeparator;
            });

            if (!string.IsNullOrEmpty(ingredients)) {
                ingredients = ingredients.Substring(0, ingredients.Length - ingredientSeparator.Length);
            }
            
            recipeText = recipeText.Replace(recipeIngredientsKey, ingredients);

            return recipeText;
        }
    }
}
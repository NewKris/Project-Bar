using System.Collections.Generic;
using Runtime.Drinks;
using UnityEngine;

namespace Runtime.UI.NewRecipeBook {
    public class RecipeBookV2 : MonoBehaviour {
        public Recipe[] initialRecipes;

        private HashSet<Recipe> _recipes;
        
        public void AddRecipes(params Recipe[] newRecipes) {
            foreach (Recipe recipe in newRecipes) {
                _recipes.Add(recipe);
            }
        }

        public void ShowNext() {
            Debug.Log("ShowNext");
        }
        
        public void ShowPrevious() {
            Debug.Log("ShowPrevious");
        }

        private void Awake() {
            _recipes = new HashSet<Recipe>();
        }
    }
}
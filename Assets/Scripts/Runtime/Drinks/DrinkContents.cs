using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Drinks {
    [Serializable]
    public struct DrinkContents {
        public DrinkContainer drinkContainer;
        public List<IngredientGroup> ingredientGroups;

        public int IngredientCount => ingredientGroups.Sum(x => x.Length);
        public List<Ingredient> AllIngredients => ingredientGroups.SelectMany(x => x.ingredients).ToList();

        public bool Contains(Ingredient ingredient) {
            return ingredientGroups.Any(x => x.ingredients.Contains(ingredient));
        }
        
        public void Clear() {
            ingredientGroups.Clear();
        }
        
        public bool ContainsLiquid() {
            return ingredientGroups.Any(x => x.ContainsLiquid());
        }
        
        public bool ContainsPoison() {
            return ingredientGroups.Any(x => x.ContainsPoison());
        }
        
        public bool DrinkIsAccepted(List<Recipe> acceptedRecipes, Action<string> onFailCallback = null) {
            return true;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Drinks {
    [Serializable]
    public class DrinkContents {
        public DrinkContainer drinkContainer;
        public List<IngredientGroup> ingredientGroups;

        public int Count => ingredientGroups.Count;
        public int IngredientCount => ingredientGroups.Sum(x => x.Length);
        public List<Ingredient> AllIngredients => ingredientGroups.SelectMany(x => x.ingredients).ToList();

        public void Validate() {
            foreach (IngredientGroup group in ingredientGroups) {
                group.Validate();
            }
        }
        
        public bool Contains(Ingredient ingredient) {
            return ingredientGroups.Any(x => x.ingredients.Contains(ingredient));
        }
        
        public bool Contains(Ingredient[] ingredients) {
            return ingredientGroups.Any(x => x.ingredients.Any(ingredients.Contains));
        }

        public void Add(IngredientGroup group) {
            ingredientGroups.Add(group);
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
            List<Recipe> eligibleRecipes = new List<Recipe>(acceptedRecipes);

            if (!EligibleContainer(eligibleRecipes)) {
                onFailCallback?.Invoke("Wrong glass!");
                return false;
            }

            if (!EligibleIngredients(eligibleRecipes)) {
                onFailCallback?.Invoke("Wrong ingredients!");
                return false;
            }

            if (!EligibleGroups(eligibleRecipes)) {
                onFailCallback?.Invoke("Wrong preparation!");
                return false;
            }
            
            return true;
        }

        private bool EligibleGroups(List<Recipe> eligibleRecipes) {
            eligibleRecipes.RemoveAll(x => x.contents.Count != Count);

            List<Recipe> closed = new List<Recipe>();
            foreach (Recipe eligibleRecipe in eligibleRecipes) {
                bool eligible = true;
                
                foreach (IngredientGroup group in ingredientGroups) {
                    eligible &= eligibleRecipe.contents.ingredientGroups.Any(x => x.ContainsSameIngredients(group));
                }

                if (!eligible) closed.Add(eligibleRecipe);
            }

            eligibleRecipes.RemoveAll(x => closed.Contains(x));
            return eligibleRecipes.Count != 0;
        }

        private bool EligibleIngredients(List<Recipe> eligibleRecipes) {
            List<Ingredient> allIngredients = AllIngredients;
            eligibleRecipes.RemoveAll(x => !x.contents.AllIngredients.SequenceEqual(allIngredients));
            return eligibleRecipes.Count != 0;
        }

        private bool EligibleContainer(List<Recipe> eligibleRecipes) {
            eligibleRecipes.RemoveAll(x => x.contents.drinkContainer != drinkContainer);
            return eligibleRecipes.Count != 0;
        }
    }
}
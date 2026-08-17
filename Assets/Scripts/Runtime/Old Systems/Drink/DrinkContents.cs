using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    [Obsolete]
    [Serializable]
    public struct DrinkContents {
        public MixType mixType;
        public DrinkContainer drinkContainer;
        public List<Ingredient> ingredients;
        [HideInInspector] public bool isDestroyed;

        public int IngredientCount => ingredients.Count;
        
        public bool ContainsLiquid() {
            return ingredients.Any(x => x.type is IngredientType.liquid);
        }
        
        public bool ContainsPoison() {
            return ingredients.Any(x => x.isPoisonous);
        }

        public bool ContainsPrepOrGarnish() {
            return ingredients.Any(x => x.type is IngredientType.prep or IngredientType.garnish);
        }
        
        public bool DrinkIsAccepted(List<Recipe> acceptedRecipes, Action<string> onFailCallback = null) {
            if (isDestroyed) {
                Debug.Log("Drink mismatch: Contains destroyed ingredients");
                onFailCallback?.Invoke("Sloppy preparation");
                return false;
            }
            
            List<Recipe> possibleRecipes = acceptedRecipes;
            
            possibleRecipes = GetRecipesWithEligibleContainer(drinkContainer, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: Container");
                onFailCallback?.Invoke("Wrong glass");
                return false;
            }
            
            possibleRecipes = GetRecipesWithEligibleMixType(mixType, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: MixType");
                onFailCallback?.Invoke("Wrong mix");
                return false;
            }
            
            possibleRecipes = GetRecipesWithEligibleIngredients(ingredients, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: Ingredients");
                onFailCallback?.Invoke("Wrong ingredients");
                return false;
            }

            /*if (!CheckForCorrectOrderOfIngredients(ingredients)) {
                Debug.Log("Wrong order of ingredients");
                return false;
            }*/
            
            return true;
        }
        
        private List<Recipe> GetRecipesWithEligibleContainer(DrinkContainer container, List<Recipe> recipes) {
            return recipes
                .Where(recipe => recipe.contents.drinkContainer == container)
                .ToList();
        }

        private List<Recipe> GetRecipesWithEligibleMixType(MixType mix, List<Recipe> recipes) {
            return recipes
                .Where(recipe => recipe.contents.mixType == mix)
                .ToList();
        }

        private List<Recipe> GetRecipesWithEligibleIngredients(List<Ingredient> currentIngredients, List<Recipe> recipes) {
            return recipes
                .Where(recipe => recipe.contents.ingredients.Count == currentIngredients.Count
                    && recipe.contents.ingredients.All(currentIngredients.Contains)
                ).ToList();
        }

        private bool CheckForCorrectOrderOfIngredients(List<Ingredient> currentIngredients) {
            IngredientType typeOfPreviousIngredient = currentIngredients[0].type;

            for (int i = 1; i < currentIngredients.Count; i++) {
                if (currentIngredients[i].type < typeOfPreviousIngredient) 
                    return false;
            }

            return true;
        }
    }
}
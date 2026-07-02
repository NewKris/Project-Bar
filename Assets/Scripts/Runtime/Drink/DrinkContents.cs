using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Drink {
    [Serializable]
    public struct DrinkContents {
        public MixType mixType;
        public DrinkContainer drinkContainer;
        public List<Ingredient> ingredients;
        
        public bool DrinkIsAccepted(List<Recipe> acceptedRecipes) {
            List<Recipe> possibleRecipes = acceptedRecipes;

            possibleRecipes = GetRecipesWithEligibleContainer(drinkContainer, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: Container");
                return false;
            }
            
            possibleRecipes = GetRecipesWithEligibleMixType(mixType, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: MixType");
                return false;
            }
            
            possibleRecipes = GetRecipesWithEligibleIngredients(ingredients, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: Ingredients");
                return false;
            }

            if (!CheckForCorrectOrderOfIngredients(ingredients)) {
                Debug.Log("Wrong order of ingredients");
                return false;
            }
            
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
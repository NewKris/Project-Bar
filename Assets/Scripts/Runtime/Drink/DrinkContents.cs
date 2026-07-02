using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Drink {
    [Serializable]
    public struct DrinkContents {
        public List<Ingredient> ingredients;
        public MixType mixType;
        [HideInInspector] public bool autoFail;

        public bool DrinkIsAccepted(List<Recipe> acceptedRecipes) {
            List<Recipe> possibleRecipes = acceptedRecipes;

            if (autoFail) return false;
            
            possibleRecipes = CheckForMatchingContainer(GetContainer(), possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: Container");
                return false;
            }
            
            possibleRecipes = CheckForMatchingMixType(mixType, possibleRecipes);
            if (possibleRecipes.Count == 0)
            {
                Debug.Log("Drink mismatch: MixType");
                return false;
            }
            
            possibleRecipes = CheckForMatchingIngredients(ingredients, possibleRecipes);
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
        
        public DrinkContainer GetContainer() {
            return ingredients.First(x => x is DrinkContainer) as DrinkContainer;
        }
        
        private List<Recipe> CheckForMatchingContainer(DrinkContainer container, List<Recipe> recipes)
        {
            List<Recipe> possibleRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.contents.ingredients.Contains(container)) possibleRecipes.Add(recipe);
            }

            return possibleRecipes;
        }

        private List<Recipe> CheckForMatchingMixType(MixType mixType, List<Recipe> recipes)
        {
            List<Recipe> possibleRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.contents.mixType == mixType) possibleRecipes.Add(recipe);
            }

            return possibleRecipes;
        }

        private List<Recipe> CheckForMatchingIngredients(List<Ingredient> ingredients, List<Recipe> recipes)
        {
            List<Recipe> possibleRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                bool isPossible = true;
                foreach (Ingredient ingredient in ingredients)
                {
                    if (!recipe.contents.ingredients.Contains(ingredient))
                    {
                        isPossible = false;
                        break;
                    }
                }
                
                if (isPossible) possibleRecipes.Add(recipe);
            }
            
            return possibleRecipes;
        }

        private bool CheckForCorrectOrderOfIngredients(List<Ingredient> ingredients)
        {
            IngredientType typeOfPreviousIngredient = ingredients[0].type;

            for (int i = 1; i < ingredients.Count; i++)
            {
                if (ingredients[i].type < typeOfPreviousIngredient) return false;
            }

            return true;
        }
    }
}
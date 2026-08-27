using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Drinks;
using UnityEngine;

namespace Runtime.Old_Systems.Looking {
    public class FlashManager : MonoBehaviour {
        private static FlashObject[] LookMaterials;

        public float flashDuration;

        public static void FlashRecipe(Recipe recipe) {
            List<Ingredient> ingredients = recipe.contents.AllIngredients;
            
            foreach (FlashObject flashObject in LookMaterials) {
                if (flashObject.ingredients == null) continue;
                
                foreach (Ingredient ingredient in flashObject.ingredients) {
                    if (ingredients.Contains(ingredient) || recipe.contents.drinkContainer == ingredient) {
                        flashObject.Flash(3);
                    }
                }
            }
        }
        
        private void Awake() {
            LookMaterials = FindObjectsByType<FlashObject>(FindObjectsSortMode.None);
        }
    }
}
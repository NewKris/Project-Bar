using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Drink {
    public class DrinkObject : MonoBehaviour {
        public DrinkContents currentContents;

        public void EmptyContents() {
            currentContents.ingredients.Clear();
            currentContents.mixType = MixType.None;
        }

        public void AddContents(DrinkContents contents) {
            currentContents.ingredients.AddRange(contents.ingredients);
            currentContents.mixType = contents.mixType;
        }
        
        public void AddIngredient(Ingredient ingredient) {
            Debug.Log($"Added {ingredient.name}");
            currentContents.ingredients.Add(ingredient);
        }
    }
}
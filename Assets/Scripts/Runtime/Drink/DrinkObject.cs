using System.Collections.Generic;
using Runtime.Configuration;
using UnityEngine;

namespace Runtime.Drink {
    public class DrinkObject : MonoBehaviour {
        public DrinkContents currentContents;
        
        protected float shakeDuration;

        public void EmptyContents() {
            currentContents.ingredients.Clear();
            currentContents.mixType = MixType.None;
            shakeDuration = 0f;
        }

        public void AddContents(DrinkContents contents) {
            currentContents.ingredients.AddRange(contents.ingredients);
            currentContents.mixType = contents.mixType;
            shakeDuration = 0f;
        }
        
        public void AddIngredient(Ingredient ingredient) {
            Debug.Log($"Added {ingredient.name}");
            currentContents.ingredients.Add(ingredient);
        }
    }
}
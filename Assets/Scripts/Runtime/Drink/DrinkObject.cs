using UnityEngine;

namespace Runtime.Drink {
    public class DrinkObject : MonoBehaviour {
        public DrinkContents currentContents;
        
        public void AddIngredient(Ingredient ingredient) {
            Debug.Log($"Added {ingredient.name}");
            currentContents.ingredients.Add(ingredient);
        }
    }
}
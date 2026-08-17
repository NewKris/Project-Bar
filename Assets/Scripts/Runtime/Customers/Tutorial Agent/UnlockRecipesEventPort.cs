using Runtime.Old_Systems.Drink;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Customers.Tutorial_Agent {
    [CreateAssetMenu(fileName = "Unlock Recipes", menuName = "Event Ports/Unlock Recipes", order = 0)]
    public class UnlockRecipesEventPort : ScriptableObject {
        public UnityAction<Recipe[]> onRecipesUnlocked;
        
        public void UnlockRecipes(Recipe[] recipes) {
            onRecipesUnlocked?.Invoke(recipes);
        }
    }
}
using FMODUnity;
using UnityEngine;

namespace Runtime.Drink {
    [CreateAssetMenu(menuName = "Drink/Ingredient")]
    public class Ingredient : ScriptableObject {
        public bool isPoisonous;
        public IngredientType type;
        public string customDisplayName;
        public EventReference ingredientSound;
        
        public string DisplayName => string.IsNullOrEmpty(customDisplayName) ? name : customDisplayName;
    }
}
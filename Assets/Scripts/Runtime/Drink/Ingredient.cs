using UnityEngine;

namespace Runtime.Drink {
    [CreateAssetMenu(menuName = "Drink/Ingredient")]
    public class Ingredient : ScriptableObject
    {
        public IngredientType type;
    }
}
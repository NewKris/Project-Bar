using UnityEngine;

namespace Runtime.Drink {
    [CreateAssetMenu(menuName = "Drink/Recipe")]
    public class Recipe : ScriptableObject {
        public DrinkContents contents;
    }
}
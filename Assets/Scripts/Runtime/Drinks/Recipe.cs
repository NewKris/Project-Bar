using System;
using UnityEngine;

namespace Runtime.Drinks {
    [CreateAssetMenu(menuName = "Drink/Recipe")]
    public class Recipe : ScriptableObject {
        public DrinkContents contents;
        public bool isAlcoholic = true;

        [Header("UI")] 
        public Sprite icon;
        public string customDisplayName;
        [TextArea] public string description;
        
        public string DisplayName => string.IsNullOrEmpty(customDisplayName) ? name : customDisplayName;

        public static int CompareAlcohol(Recipe a, Recipe b) {
            return a.isAlcoholic ? 1 : -1;
        }
        
        public static int CompareContainer(Recipe a, Recipe b) {
            return String.Compare(a.contents.drinkContainer.name, b.contents.drinkContainer.name, StringComparison.Ordinal);
        }
        
        public static int CompareName(Recipe a, Recipe b) {
            return String.Compare(a.name, b.name, StringComparison.Ordinal);
        }
        
        private void OnValidate() {
            contents.Validate();
        }
    }
}
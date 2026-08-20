using System;
using FMODUnity;
using Runtime.Drinks.Converting;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Drinks {
    [CreateAssetMenu(menuName = "Drink/Ingredient")]
    public class Ingredient : ScriptableObject {
        public bool isPoisonous;
        public IngredientType type;
        public EventReference ingredientSound;
        public Conversion[] ingredientInteractions;

        [Header("UI")] 
        public string customDisplayName;
        [TextArea] public string description;
        [ImagePreview] public Sprite icon;
        
        public string DisplayName => string.IsNullOrEmpty(customDisplayName) ? name : customDisplayName;

        private void OnValidate() {
            if (ingredientInteractions == null) return;
            
            for (var i = 0; i < ingredientInteractions.Length; i++) {
                ingredientInteractions[i].UpdateName();
            }
        }
    }
}
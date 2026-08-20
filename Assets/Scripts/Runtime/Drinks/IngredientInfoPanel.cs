using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Drinks {
    public class IngredientInfoPanel : MonoBehaviour {
        private static event Action<Ingredient> LookIngredientsChanged;
        
        public TMP_Text title;
        public TMP_Text description;
        public Image icon;

        public static void SetLookedIngredient(Ingredient ingredient) {
            LookIngredientsChanged?.Invoke(ingredient);
        }

        private void Awake() {
            LookIngredientsChanged += DrawInfo;
        }

        private void OnDestroy() {
            LookIngredientsChanged -= DrawInfo;
        }

        private void DrawInfo(Ingredient ingredient) {
            title.text = ingredient.DisplayName;
            description.text = ingredient.description;
            icon.sprite = ingredient.icon;
        }
    }
}
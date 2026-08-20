using System;
using Runtime.Old_Systems.Looking;
using UnityEngine;

namespace Runtime.Drinks {
    public class IngredientInfoTrigger : MonoBehaviour {
        public Ingredient ingredient;

        private void Awake() {
            if (gameObject.TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(() => IngredientInfoPanel.SetLookedIngredient(ingredient));
            }
        }
    }
}
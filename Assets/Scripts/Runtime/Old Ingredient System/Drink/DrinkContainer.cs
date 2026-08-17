using System;
using UnityEngine;

namespace Runtime.Drink {
    [CreateAssetMenu(menuName =  "Drink/Drink Container")]
    public class DrinkContainer : Ingredient {
        private void Reset() {
            type = IngredientType.container;
        }
    }
}
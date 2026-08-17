using System;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    [CreateAssetMenu(menuName =  "Drink/Drink Container")]
    [Obsolete]
    public class DrinkContainer : Ingredient {
        private void Reset() {
            type = IngredientType.container;
        }
    }
}
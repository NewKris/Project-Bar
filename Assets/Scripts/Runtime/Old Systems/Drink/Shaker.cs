using System;
using System.Linq;
using Runtime.Drinks;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    [Serializable]
    public struct ShakerConversion {
        public Ingredient from;
        public Ingredient to;
    }
    
    [Obsolete]
    public class Shaker : DrinkObject {
        public float shakeDuration = 0.5f;
        public ShakerConversion[] conversions;
        
        public void TickShake() {
            ShakeDuration += Time.deltaTime;
            if (ShakeDuration >= shakeDuration) {
                GroupIngredients();
            }
        }

        private void GroupIngredients() {
            IngredientGroup shakerGroup = IngredientGroup
                .CreateNewGroup(currentContents.ingredientGroups.SelectMany(x => x.ingredients).ToArray());
            
            currentContents.Clear();
            currentContents.Add(shakerGroup);
            
            foreach (ShakerConversion conversion in conversions) {
                if (shakerGroup.ingredients.Contains(conversion.from)) {
                    shakerGroup.ingredients.Remove(conversion.from);
                    shakerGroup.ingredients.Add(conversion.to);
                }
            }
        }
    }
}
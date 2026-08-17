using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Old_Systems.Drink {
    [Serializable]
    public struct IngredientGroup {
        public List<Ingredient> ingredients;
        
        public int Length => ingredients.Count;
        public Ingredient this[int i]  => ingredients[i];

        public static IngredientGroup CreateNewGroup(params Ingredient[] ingredients) {
            return new IngredientGroup() {
                ingredients = new List<Ingredient>(ingredients)
            };
        }
        
        public bool ContainsLiquid() {
            return ingredients.Any(x => x.type is IngredientType.liquid);
        }

        public bool ContainsPoison() {
            return ingredients.Any(x => x.isPoisonous);
        }
    }
}
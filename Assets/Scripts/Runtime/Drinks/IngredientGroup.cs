using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Drinks {
    [Serializable]
    public class IngredientGroup {
        [HideInInspector] public string groupName;
        public List<Ingredient> ingredients;
        
        public int Length => ingredients.Count;
        public Ingredient this[int i]  => ingredients[i];

        public static IngredientGroup CreateNewGroup(params Ingredient[] ingredients) {
            return new IngredientGroup() {
                ingredients = new List<Ingredient>(ingredients)
            };
        }

        public bool ContainsSameIngredients(IngredientGroup otherGroup) {
            return otherGroup != null && otherGroup.ingredients.SequenceEqual(ingredients);
        }

        public void Validate() {
            groupName = ingredients.Count > 0 ? CompileNames() : "No ingredients added to group...";
        }
        
        public bool ContainsLiquid() {
            return ingredients.Any(x => x.type is IngredientType.liquid);
        }

        public bool ContainsPoison() {
            return ingredients.Any(x => x.isPoisonous);
        }

        private string CompileNames() {
            string result = "";
            string separator = ", ";

            for (int i = 0; i < ingredients.Count; i++) {
                result += ingredients[i]?.name ?? "NULL";

                if (i != ingredients.Count - 1) {
                    result += separator;
                }
            }

            return result;
        }
    }
}
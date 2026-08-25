using System.Collections.Generic;
using Runtime.Drinks;
using Runtime.Old_Systems.Drink;
using Runtime.Old_Systems.Items;
using Runtime.UI;
using UnityEngine;

namespace Runtime.Items {
    public class DrinkChecker : Old_Systems.Items.ItemDock {
        public Recipe recipeToValidate;
        
        public override void PlaceItem(ItemPickup item) {
            base.PlaceItem(item);

            if (item.TryGetComponent(out DrinkObject drink)) {
                VerifyRecipe(drink);
            }
        }

        private void VerifyRecipe(DrinkObject drink) {
            bool drinkPasses = drink.currentContents.DrinkIsAccepted(new List<Recipe>() { recipeToValidate }, msg => {
                WorldSpaceCanvas.SpawnBarkText(msg, transform.position, transform.rotation, Color.red);
            });

            if (drinkPasses) {
                WorldSpaceCanvas.SpawnBarkText("Success!", transform.position, transform.rotation, Color.green);
            }
        }
    }
}
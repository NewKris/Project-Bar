using Runtime.Drink;
using UnityEngine;

namespace Runtime.Stations {
    public class FillStation : Station<DrinkObject> {
        public Ingredient ingredient;

        private DrinkObject _drink;
        
        public override void StartStation() {
            if (itemDock.HeldItem?.TryGetComponent(out _drink) ?? false) {
                enabled = true;
                itemDock.HeldItem.SetInteractable(false);
                
            }
        }
        
        public override void StopStation() {
            
        }
    }
}
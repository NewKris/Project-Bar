using Runtime.Drink;
using UnityEngine;

namespace Runtime.Stations {
    public class FillStation : Station<DrinkObject> {
        public Ingredient ingredient;

        private DrinkObject _drink;
        
        public override void StartStation() {
            StartStationTimer();
        }
        
        public override void StopStation() {
            enabled = false;
            itemDock.HeldItem?.SetInteractable(true);
        }

        protected override void OnReachedEndState() { }
        
        protected override void OnReachedMiddleState() { }
    }
}
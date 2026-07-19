using NaughtyAttributes;
using Runtime.Animations;
using Runtime.Drink;
using UnityEngine;

namespace Runtime.Stations {
    public class FillStation : Station {
        public Ingredient ingredient;
        public float fillDuration;
        [Foldout("References")] public RumbleAnimation rumble;

        public override void StartStation() {
            if (AlreadyContainsIngredient()) {
                Debug.Log($"{gameObject.name} already contains ingredient");
                return;
            }
            
            StartStationTimer();
        }
        
        public override void StopStation() {
            enabled = false;
            itemDock.HeldItem?.SetInteractable(true);
        }
        
        private void OnEnable() {
            rumble.Shaking = true;
        }

        private void OnDisable() {
            rumble.Shaking = false;
        }

        protected override void Update() {
            base.Update();

            if (currentItem.GetStationTime(stationKey) > fillDuration) {
                AddIngredient();
            }
        }

        private bool AlreadyContainsIngredient() {
            return itemDock.HeldItem != null 
                   && itemDock.HeldItem.TryGetComponent(out DrinkObject drink) 
                   && drink.currentContents.ingredients.Contains(ingredient);
        }

        private void AddIngredient() {
            currentItem.AddIngredient(ingredient);
            currentItem.RemoveStationKey(stationKey);
        }
    }
}
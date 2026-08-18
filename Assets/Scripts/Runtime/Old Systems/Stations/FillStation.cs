using System;
using Runtime.Audio;
using Runtime.Drinks;
using Runtime.Old_Systems.Drink;
using UnityEngine;

namespace Runtime.Old_Systems.Stations {
    [Obsolete]
    public class FillStation : Station {
        public Ingredient ingredient;
        public float fillDuration;

        public override void StartStation() {
            if (isToggle && IsActive) {
                TurnStationOff();
            }
            else {
                if (AlreadyContainsIngredient()) {
                    Debug.Log($"{gameObject.name} already contains ingredient");
                    return;
                }
            
                SfxManager.StartAudio(stationAudioKey, stationAudio, transform.position);
            
                StartStationTimer();
            }
        }
        
        public override void StopStation() {
            if (!isToggle) TurnStationOff();
        }

        private bool IsDone() {
            return currentItem.GetStationTime(stationKey) > fillDuration;
        }
        
        protected override float MaxFill() {
            return fillDuration;
        }
        
        protected override void Update() {
            base.Update();

            if (IsDone()) {
                AddIngredient();
                TurnStationOff();
            }
        }

        private bool AlreadyContainsIngredient() {
            return itemDock.HeldItem != null 
                   && itemDock.HeldItem.TryGetComponent(out DrinkObject drink) 
                   && drink.currentContents.Contains(ingredient);
        }

        private void AddIngredient() {
            currentItem.AddIngredient(ingredient, true);
            currentItem.RemoveStationKey(stationKey);
        }

        private void TurnStationOff() {
            enabled = false;
            itemDock.HeldItem?.SetInteractable(true);
            
            SfxManager.StopAudio(stationAudioKey);
        }
    }
}
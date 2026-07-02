using System;
using System.Linq;
using Runtime.Animations;
using Runtime.Drink;
using Runtime.Items;
using Runtime.Utility.Extensions;
using UnityEngine;

namespace Runtime.Stations {
    [Serializable]
    public struct Conversion {
        public Ingredient startState;
        public Ingredient middleState;
        public Ingredient endState;
    }
    
    public class MixerStation : Station<DrinkObject> {
        public RumbleAnimation rumble;
        
        public Conversion[] conversions;

        
        public override void StartStation() {
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

        private void ConvertToMiddleStates() {
            foreach (Conversion conversion in conversions) {
                TryConvert(CurrentItem, conversion.startState, conversion.middleState);
            }
        }

        private void ConvertToEndStates() {
            foreach (Conversion conversion in conversions) {
                TryConvert(CurrentItem, conversion.middleState, conversion.endState);
            }
            
            CurrentItem.RemoveStationKey(StationKey);
        }

        private void TryConvert(DrinkObject drink, Ingredient from, Ingredient to) {
            if (!drink.currentContents.ingredients.Contains(from)) return;
            
            int convertCount = drink.currentContents.ingredients.Count(x => x == from);
            
            drink.currentContents.ingredients.RemoveAll(x => x == from);
            drink.currentContents.ingredients.AddAmount(to, convertCount);
        }
    }
}
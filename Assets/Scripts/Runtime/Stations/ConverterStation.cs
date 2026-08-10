using System;
using System.Linq;
using NaughtyAttributes;
using Runtime.Animations;
using Runtime.Audio;
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
    
    public class ConverterStation : Station {
        public float  middleStateDuration;
        public float  endStateDuration;
        
        public Conversion[] conversions;

        public override void StartStation() {
            if (isToggle && IsActive) {
                TurnStationOff();
            }
            else {
                StartStationTimer();
                SfxManager.StartAudio(stationAudioKey, stationAudio, transform.position);
            }
        }

        public override void StopStation() {
            if (!isToggle) TurnStationOff();
        }
        
        private bool IsDone() {
            return currentItem.GetStationTime(stationKey) >= endStateDuration;
        }

        protected override float MaxFill() {
            return endStateDuration;
        }

        protected override void Update() {
            base.Update();
            
            if (currentItem.GetStationTime(stationKey) >= middleStateDuration) {
                ConvertToMiddleStates();
            }

            if (IsDone()) {
                ConvertToEndStates();
                TurnStationOff();
            }
        }

        private void ConvertToMiddleStates() {
            foreach (Conversion conversion in conversions) {
                TryConvertIngredients(currentItem, conversion.startState, conversion.middleState);
            }
        }

        private void ConvertToEndStates() {
            foreach (Conversion conversion in conversions) {
                if (conversion.startState is DrinkContainer container) {
                    TryConvertContainer(
                        currentItem, 
                        container, 
                        container
                    );
                }
                else {
                    TryConvertIngredients(
                        currentItem, 
                        conversion.middleState, 
                        conversion.endState
                    );
                }
            }
            
            currentItem.RemoveStationKey(stationKey);
        }

        private void TryConvertIngredients(DrinkObject drink, Ingredient from, Ingredient to) {
            if (!drink.currentContents.ingredients.Contains(from)) return;
            
            int convertCount = drink.currentContents.ingredients.Count(x => x == from);
            
            drink.currentContents.ingredients.RemoveAll(x => x == from);
            drink.currentContents.ingredients.AddAmount(to, convertCount);
        }

        private void TryConvertContainer(DrinkObject drink, DrinkContainer from, DrinkContainer to) {
            if (drink.currentContents.drinkContainer == from)
                drink.currentContents.drinkContainer = to;
        }

        private void TurnStationOff() {
            enabled = false;
            itemDock.HeldItem?.SetInteractable(true);
            
            SfxManager.StopAudio(stationAudioKey);
        }
    }
}
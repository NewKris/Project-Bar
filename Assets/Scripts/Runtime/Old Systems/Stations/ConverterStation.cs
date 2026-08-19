using System;
using System.Linq;
using Runtime.Audio;
using Runtime.Drinks;
using Runtime.Old_Systems.Drink;
using Runtime.Utility.Extensions;

namespace Runtime.Old_Systems.Stations {
    [Serializable]
    [Obsolete]
    public struct Conversion {
        public Ingredient from;
        public Ingredient middleState;
        public Ingredient to;
    }
    
    [Obsolete]
    public class ConverterStation : Station {
        public float  conversionTime;
        public float middleStateTime;
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
            return currentItem.GetStationTime(stationKey) >= conversionTime;
        }

        private bool ReachedMiddleState() {
            return currentItem.GetStationTime(stationKey) >= middleStateTime;
        }

        protected override float MaxFill() {
            return conversionTime;
        }

        protected override void Update() {
            base.Update();

            if (ReachedMiddleState()) {
                ConvertToMiddleStates();
            }

            if (IsDone()) {
                ConvertToEndStates();
                TurnStationOff();
            }
        }

        private void ConvertToMiddleStates() {
            foreach (Conversion conversion in conversions) {
                if (conversion is { from: DrinkContainer startContainer, middleState: DrinkContainer middleContainer }) {
                    TryConvertContainer(
                        currentItem, 
                        startContainer, 
                        middleContainer
                    );
                }
                else {
                    TryConvertIngredients(
                        currentItem, 
                        conversion.from, 
                        conversion.middleState
                    );
                }
            }
        }
        
        private void ConvertToEndStates() {
            foreach (Conversion conversion in conversions) {
                if (conversion is { middleState: DrinkContainer middleContainer, to: DrinkContainer endContainer }) {
                    TryConvertContainer(
                        currentItem, 
                        middleContainer, 
                        endContainer
                    );
                }
                else {
                    TryConvertIngredients(
                        currentItem, 
                        conversion.middleState, 
                        conversion.to
                    );
                }
            }
            
            currentItem.RemoveStationKey(stationKey);
        }

        private void TryConvertIngredients(DrinkObject drink, Ingredient from, Ingredient to) {
            foreach (IngredientGroup group in drink.currentContents.ingredientGroups) {
                int addAmount = group.ingredients.Count(x => x == from);
                
                group.ingredients.RemoveAll(x => x == from);
                group.ingredients.AddAmount(to, addAmount);
            }
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
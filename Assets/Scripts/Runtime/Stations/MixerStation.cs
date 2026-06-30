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
    
    public class MixerStation : MonoBehaviour {
        public ItemDock itemDock;
        public RumbleAnimation rumble;
        public float minMixDuration;
        public float maxMixDuration;
        public Conversion[] conversions;

        private int _stationKey;
        private Shaker _currentShaker;
        
        public void StartMixer() {
            if (itemDock.HeldItem?.TryGetComponent(out _currentShaker) ?? false) {
                enabled = true;
                itemDock.HeldItem.SetInteractable(false);

                if (!_currentShaker.HasMixTimer(_stationKey)) {
                    _currentShaker.CreateMixTimer(_stationKey);
                }
            }
        }

        public void StopMixer() {
            enabled = false;
            itemDock.HeldItem?.SetInteractable(true);
        }

        private void Awake() {
            enabled = false;
            _stationKey = gameObject.GetInstanceID();
        }

        private void OnEnable() {
            rumble.Shaking = true;
        }

        private void OnDisable() {
            rumble.Shaking = false;
        }

        private void Update() {
            _currentShaker.TickMix(_stationKey);
            
            if (_currentShaker.GetMixAmount(_stationKey) >= minMixDuration) {
                ConvertToMiddleStates();
            }

            if (_currentShaker.GetMixAmount(_stationKey) >= maxMixDuration) {
                ConvertToEndStates();
            }
        }

        private void ConvertToMiddleStates() {
            foreach (Conversion conversion in conversions) {
                TryConvert(_currentShaker, conversion.startState, conversion.middleState);
            }
        }

        private void ConvertToEndStates() {
            foreach (Conversion conversion in conversions) {
                TryConvert(_currentShaker, conversion.middleState, conversion.endState);
            }
            
            _currentShaker.RemoveMixerKey(_stationKey);
        }

        private void TryConvert(DrinkObject drink, Ingredient from, Ingredient to) {
            if (!drink.currentContents.ingredients.Contains(from)) return;
            
            int convertCount = drink.currentContents.ingredients.Count(x => x == from);
            
            drink.currentContents.ingredients.RemoveAll(x => x == from);
            drink.currentContents.ingredients.AddAmount(to, convertCount);
        }
    }
}
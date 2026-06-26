using System;
using Runtime.Drink;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Stations {
    public class MixerStation : MonoBehaviour {
        public ItemDock itemDock;

        private Shaker _currentShaker;
        
        public void StartMixer() {
            if (itemDock.HeldItem?.TryGetComponent(out _currentShaker) ?? false) {
                enabled = true;
            }
            else {
                enabled = false;
            }
        }

        public void StopMixer() {
            enabled = false;
        }

        private void Awake() {
            enabled = false;
        }

        private void Update() {
            _currentShaker.TickShake();
        }
    }
}
using System;
using NaughtyAttributes;
using Runtime.Audio;
using Runtime.Drink;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Stations {
    public abstract class Station : MonoBehaviour {
        [Foldout("References")] public ItemDock itemDock;
        [Foldout("References")] public StationAudio stationAudio;

        protected DrinkObject currentItem;
        protected int stationKey;
        
        public abstract void StartStation();
        public abstract void StopStation();

        protected void StartStationTimer() {
            if (itemDock.HeldItem?.TryGetComponent(out currentItem) ?? false) {
                enabled = true;
                itemDock.HeldItem.SetInteractable(false);

                if (!currentItem.HasStationTimer(stationKey)) {
                    currentItem.CreateStationTimer(stationKey);
                }
            }
        }

        private void Reset() {
            gameObject.layer = LayerMask.NameToLayer("Station");
        }

        private void Awake() {
            enabled = false;
            stationKey = gameObject.GetInstanceID();
        }
        
        protected virtual void Update() {
            currentItem.TickStationTimer(stationKey);
        }
    }
}
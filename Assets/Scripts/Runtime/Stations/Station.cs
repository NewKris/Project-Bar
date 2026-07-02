using System;
using Runtime.Drink;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Stations {
    public abstract class Station<T> : MonoBehaviour where T : DrinkObject {
        public ItemDock itemDock;
        public float middleStateDuration;
        public float endStateDuration;

        protected T currentItem;
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
        
        private void Awake() {
            enabled = false;
            stationKey = gameObject.GetInstanceID();
        }
        
        private void Update() {
            currentItem.TickStationTimer(stationKey);
            
            if (currentItem.GetStationTime(stationKey) >= middleStateDuration) {
                OnReachedMiddleState();
            }

            if (currentItem.GetStationTime(stationKey) >= endStateDuration) {
                OnReachedEndState();
            }
        }
        
        protected abstract void OnReachedEndState();
        protected abstract void OnReachedMiddleState();
    }
}
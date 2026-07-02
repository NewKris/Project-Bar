using System;
using Runtime.Drink;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Stations {
    public abstract class Station<T> : MonoBehaviour where T : DrinkObject {
        public ItemDock itemDock;
        public float middleStateDuration;
        public float endStateDuration;

        protected T CurrentItem;
        protected int StationKey;
        protected event Action OnReachedMiddleState;
        protected event Action OnReachedEndState;
        
        public abstract void StartStation();
        public abstract void StopStation();

        protected void StartStationTimer() {
            if (itemDock.HeldItem?.TryGetComponent(out CurrentItem) ?? false) {
                enabled = true;
                itemDock.HeldItem.SetInteractable(false);

                if (!CurrentItem.HasStationTimer(StationKey)) {
                    CurrentItem.CreateStationTimer(StationKey);
                }
            }
        }
        
        private void Awake() {
            enabled = false;
            StationKey = gameObject.GetInstanceID();
        }
        
        private void Update() {
            CurrentItem.TickStationTimer(StationKey);
            
            if (CurrentItem.GetStationTime(StationKey) >= middleStateDuration) {
                OnReachedMiddleState?.Invoke();
            }

            if (CurrentItem.GetStationTime(StationKey) >= endStateDuration) {
                OnReachedEndState?.Invoke();
            }
        }
    }
}
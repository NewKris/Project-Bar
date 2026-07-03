using System;
using Runtime.Drink;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Stations {
    public abstract class Station<T> : MonoBehaviour where T : DrinkObject {
        public ItemDock itemDock;

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
        
        protected virtual void Update() {
            currentItem.TickStationTimer(stationKey);
        }
    }
}
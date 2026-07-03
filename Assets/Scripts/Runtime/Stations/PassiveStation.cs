using System;
using System.Linq;
using Runtime.Drink;
using Runtime.Interact;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Stations {
    [RequireComponent(typeof(HandInteraction))]
    public class PassiveStation : MonoBehaviour {
        private ItemDock[] _docks;

        public void PlaceItem(ItemPickup item) {
            FindFirstAvailableSlot()?.PlaceItem(item);
        }

        public bool CanPlaceItem() {
            return _docks.Any(x => x.HeldItem == null);
        }

        private void Awake() {
            _docks = GetComponentsInChildren<ItemDock>();
        }

        private void Update() {
            
        }

        private ItemDock FindFirstAvailableSlot() {
            return _docks.FirstOrDefault(x => x.HeldItem == null);
        }
    }
}
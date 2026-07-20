using System.Linq;
using Runtime.Interact;
using Runtime.Player.Hand;
using UnityEngine;

namespace Runtime.Items {
    [RequireComponent(typeof(HandInteraction))]
    public class MultiDock : MonoBehaviour {
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

        private ItemDock FindFirstAvailableSlot() {
            return _docks.FirstOrDefault(x => x.HeldItem == null);
        }
    }
}
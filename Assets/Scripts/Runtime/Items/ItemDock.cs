using System;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Items {
    public class ItemDock : MonoBehaviour {
        public Transform itemPivot;

        private ItemPickup _heldItem;

        public bool CanPlaceItem() {
            return _heldItem == null;
        }
        
        public void PlaceItem(ItemPickup item) {
            item.Pin(itemPivot);
            _heldItem = item;
            _heldItem.onPinned += RemoveItem;
        }

        private void RemoveItem() {
            _heldItem.onPinned -= RemoveItem;
            _heldItem = null;
        }

        private void OnDrawGizmos() {
            if (!itemPivot) return;
            
            HandlesProxy.DrawDisc(itemPivot.position, Vector3.up, 0.25f, false, Color.yellow);
        }
    }
}
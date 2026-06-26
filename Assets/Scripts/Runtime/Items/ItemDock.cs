using System;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Items {
    public class ItemDock : MonoBehaviour {
        public Transform itemPivot;

        public ItemPickup HeldItem { get; private set; }

        public bool CanPlaceItem() {
            return HeldItem == null;
        }
        
        public void PlaceItem(ItemPickup item) {
            item.Pin(itemPivot);
            HeldItem = item;
            HeldItem.OnPinned += RemoveItem;
        }

        private void RemoveItem() {
            HeldItem.OnPinned -= RemoveItem;
            HeldItem = null;
        }

        private void OnDrawGizmos() {
            if (!itemPivot) return;
            
            HandlesProxy.DrawDisc(itemPivot.position, Vector3.up, 0.25f, false, Color.yellow);
        }
    }
}
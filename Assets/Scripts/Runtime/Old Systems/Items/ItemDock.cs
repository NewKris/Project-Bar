using System;
using Runtime.Audio;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Old_Systems.Items {
    [Obsolete]
    public class ItemDock : MonoBehaviour {
        public Transform itemPivot;
        public SurfaceMaterialType surfaceLabel;

        public ItemPickup HeldItem { get; private set; }

        private ItemPickup _exclusiveItem;

        public bool CanPlaceItem(ItemPickup item) {
            return HeldItem == null && (_exclusiveItem == null || _exclusiveItem == item);
        }

        public void SetExclusiveItem(ItemPickup item) {
            _exclusiveItem = item;
        }
        
        public void PlaceItem(ItemPickup item) {
            item.Pin(itemPivot);
            HeldItem = item;
            HeldItem.PlayPutDownSound(surfaceLabel.ToString());
            HeldItem.OnPinned += RemoveItem;
        }

        private void RemoveItem() {
            HeldItem.OnPinned -= RemoveItem;
            HeldItem = null;
        }

        private void OnDrawGizmos() {
            if (!itemPivot) return;
            
            HandlesProxy.DrawDisc(itemPivot.position, Vector3.up, 0.05f, false, Color.yellow);
        }
    }
}
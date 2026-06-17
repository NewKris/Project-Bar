using System;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerHand : MonoBehaviour {
        private ItemPickup _heldItem;
        
        private void Awake() {
            ItemPickup.OnPickup += PickUpItem;
        }

        private void OnDestroy() {
            ItemPickup.OnPickup -= PickUpItem;
        }

        public void ReleaseHeldItem() {
            _heldItem?.Unpin();
            _heldItem = null;
        }

        private void PickUpItem(ItemPickup item) {
            if (_heldItem) return;
            
            _heldItem = item;
            item.Pin(transform);
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.25f);
        }
    }
}
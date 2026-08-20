using System;
using NaughtyAttributes;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Items {
    public class ItemDock : MonoBehaviour {
        [ReadOnly] public ItemObject heldItem;
        public Transform pin;

        private Transform Pin => pin == null ? transform : pin;
        
        public void HoldItem(ItemObject item) {
            heldItem = item;
            item.Pin(Pin);
        }
        
        public void ReleaseItem() {
            heldItem.UnPin();
            heldItem = null;
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawDisc(Pin.position, Vector3.up, 0.05f, false, Color.yellow);
        }
    }
}
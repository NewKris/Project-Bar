using System;
using Runtime.Common;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerHand : MonoBehaviour {
        private void Awake() {
            ItemPickup.OnPickup += PickUpItem;
        }

        private void OnDestroy() {
            ItemPickup.OnPickup -= PickUpItem;
        }

        private void PickUpItem(ItemPickup item) {
            
        }
    }
}
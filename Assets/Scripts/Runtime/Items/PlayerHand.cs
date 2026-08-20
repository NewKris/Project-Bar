using System;
using UnityEngine;

namespace Runtime.Items {
    public class PlayerHand : ItemDock {
        private void Awake() {
            ItemObject.OnItemGrabbed += TryGrabItem;
        }

        private void OnDestroy() {
            ItemObject.OnItemGrabbed -= TryGrabItem;
        }

        private void TryGrabItem(ItemObject item) {
            if (!heldItem) {
                HoldItem(item);
            }
        }
    }
}
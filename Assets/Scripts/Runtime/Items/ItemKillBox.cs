using System;
using UnityEngine;

namespace Runtime.Items {
    public class ItemKillBox : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out ItemPickup pickup)) {
                pickup.BreakItem();
            }
        }
    }
}
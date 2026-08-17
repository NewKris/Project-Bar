using System;
using UnityEngine;

namespace Runtime.Old_Systems.Items {
    [Obsolete]
    public class ItemKillBox : MonoBehaviour {
        private void OnTriggerEnter(Collider other) {
            if (other.TryGetComponent(out ItemPickup pickup)) {
                pickup.BreakItem();
            }
        }
    }
}
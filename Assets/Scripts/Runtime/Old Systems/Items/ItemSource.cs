using System;
using UnityEngine;

namespace Runtime.Old_Systems.Items {
    [Obsolete]
    public class ItemSource : MonoBehaviour {
        public GameObject itemPrefab;

        public ItemPickup SpawnItem() {
            ItemPickup pickup = Instantiate(itemPrefab).GetComponent<ItemPickup>();
            pickup.source = this;

            return pickup;
        }
    }
}
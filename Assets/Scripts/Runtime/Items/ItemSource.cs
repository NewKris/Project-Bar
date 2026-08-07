using UnityEngine;

namespace Runtime.Items {
    public class ItemSource : MonoBehaviour {
        public GameObject itemPrefab;

        public ItemPickup SpawnItem() {
            ItemPickup pickup = Instantiate(itemPrefab).GetComponent<ItemPickup>();
            pickup.source = this;

            return pickup;
        }
    }
}
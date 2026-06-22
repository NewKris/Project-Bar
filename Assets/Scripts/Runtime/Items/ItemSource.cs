using UnityEngine;

namespace Runtime.Items {
    public class ItemSource : MonoBehaviour {
        public GameObject itemPrefab;

        public ItemPickup SpawnItem() {
            return Instantiate(itemPrefab).GetComponent<ItemPickup>();
        }
    }
}
using System;
using UnityEngine;

namespace Runtime.Common {
    public abstract class ItemPickup : MonoBehaviour {
        public static event Action<ItemPickup> OnPickup;
        
        public void PickUp() {
            OnPickup?.Invoke(this);
        }
    }
}

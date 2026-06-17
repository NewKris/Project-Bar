using System;
using UnityEngine;

namespace Runtime.Common {
    public abstract class ItemPickup : MonoBehaviour {
        public static event Action<ItemPickup> OnPickup;
        
        public void PickUp() {
            OnPickup?.Invoke(this);
        }

        public void Pin(Transform pinPoint) {
            Rigidbody rb = GetComponent<Rigidbody>();
            
            transform.SetParent(pinPoint);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            rb.isKinematic = true;
            rb.position = pinPoint.position;
            rb.rotation = pinPoint.rotation;
        }

        public void Unpin() {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.SetParent(null);
        }
    }
}

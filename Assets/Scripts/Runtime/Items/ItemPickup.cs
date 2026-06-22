using System;
using UnityEngine;

namespace Runtime.Items {
    public class ItemPickup : MonoBehaviour {
        public event Action onPinned;
        
        public void Pin(Transform pinPoint) {
            onPinned?.Invoke();
            
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

using System;
using UnityEngine;

namespace Runtime.Items {
    public class ItemPickup : MonoBehaviour {
        public event Action OnPinned;

        public void BreakItem() {
            Destroy(gameObject);
        }

        public void SetFrontRender(bool renderInFront) {
            int layer = renderInFront ? LayerMask.NameToLayer("Held Item") : LayerMask.NameToLayer("Default");
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
                renderer.gameObject.layer = layer;
            }
        }
        
        public void Pin(Transform pinPoint) {
            OnPinned?.Invoke();
            
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

using System;
using Runtime.Satisfaction;
using UnityEngine;

namespace Runtime.Items {
    public class ItemPickup : MonoBehaviour {
        public int dropPenalty;
        public SatisfactionPort satisfactionPort;
        
        public event Action OnPinned;

        public void BreakItem() {
            satisfactionPort.DecreaseSatisfaction(dropPenalty);
            Despawn();
        }

        public void Despawn() {
            Destroy(gameObject);
        }

        public void SetFrontRender(bool renderInFront) {
            int layer = renderInFront ? LayerMask.NameToLayer("Held Item") : LayerMask.NameToLayer("Default");
            foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>()) {
                renderer.gameObject.layer = layer;
            }
        }

        public void SetInteractable(bool interactable) {
            GetComponentInChildren<Collider>().enabled = interactable;
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

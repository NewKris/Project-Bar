using Runtime.Utility;
using UnityEngine;

namespace Runtime.Interact {
    public class InteractRay : MonoBehaviour {
        public float interactDistance;
        public LayerMask defaultMask;
        
        public bool TryFindInteraction<T>(out T genericInteractable) where T: MonoBehaviour {
            return TryFindInteraction(out genericInteractable, defaultMask);
        }
        
        public bool TryFindInteraction<T>(out T genericInteractable, int layerMask) where T: MonoBehaviour {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, layerMask);

            if (!hit) {
                genericInteractable = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out genericInteractable);
            return genericInteractable != null;
        }
        
        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.magenta);
        }
    }
}
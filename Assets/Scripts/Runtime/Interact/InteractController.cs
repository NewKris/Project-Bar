using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact
{
    public class InteractController : MonoBehaviour
    {
        public float interactDistance;
        public UnityEvent<Interactable> onInteract;

        public void TryInteract()
        {
            Interactable interact = TryFindInteraction(out Interactable interaction) ? interaction : null;
            onInteract.Invoke(interact);
        }
        
        private bool TryFindInteraction(out Interactable genericGenericInteractable) {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo);

            if (!hit) {
                genericGenericInteractable = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out genericGenericInteractable);
            return genericGenericInteractable != null;
        }
        
        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.magenta);
        }
    }
}
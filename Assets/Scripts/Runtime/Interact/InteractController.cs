using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact
{
    public class InteractController : MonoBehaviour
    {
        public UnityEvent<Interactable> OnInteract;

        public void TryInteract()
        {
            Interactable interact = TryFindInteraction(out Interactable interaction) ? interaction : null;
            OnInteract.Invoke(interact);
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
    }
}
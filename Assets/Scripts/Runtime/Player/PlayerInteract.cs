using Runtime.Customers;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        public void TryInteract(Interactable interactable)
        {
            Debug.Log("TRY INTERACT");
            if (interactable == null) return;

            
            if (interactable.TryGetComponent(out Customer customer))
            {
                customer.Order();
            }
            
        }
    }
}
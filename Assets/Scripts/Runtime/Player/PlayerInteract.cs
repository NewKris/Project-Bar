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
            Debug.Log($"I did find {interactable.name}");

            
            if (interactable.TryGetComponent(out Customer customer))
            {
                Debug.Log("CUSTOMER!");
                customer.Order();
            }
            
        }
    }
}
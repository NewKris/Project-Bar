using Runtime.Customers;
using Runtime.Highlighting;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        public void TryInteract(Interactable interactable)
        {
            if (interactable == null) return;
            
            if (interactable.TryGetComponent(out CustomerBase customer))
            {
                customer.Order();
            }

            if (interactable.TryGetComponent(out Highlightable highlightable)) {
                highlightable.Click();
            }
        }
    }
}
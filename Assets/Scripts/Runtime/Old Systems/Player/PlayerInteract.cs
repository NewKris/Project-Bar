using System;
using Runtime.Customers;
using Runtime.Dialogue;
using Runtime.Highlighting;
using Runtime.Old_Systems.Interact;
using UnityEngine;

namespace Runtime.Old_Systems.Player
{
    [Obsolete]
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

            if (interactable.TryGetComponent(out DialogueDisplayProgressable dialogueBox)) {
                dialogueBox.HideDialogue();
            }
        }
    }
}
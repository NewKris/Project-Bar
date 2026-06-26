using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact
{
    public class InteractController : MonoBehaviour {
        public InteractRay interactRay;
        public UnityEvent<Interactable> onInteract;

        public void TryInteract()
        {
            Interactable interact = interactRay.TryFindInteraction(out Interactable interaction) ? interaction : null;
            onInteract.Invoke(interact);
        }
    }
}
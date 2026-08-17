using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Old_Systems.Interact
{
    [Obsolete]
    public class InteractController : MonoBehaviour {
        public InteractRay interactRay;
        public UnityEvent<Interactable> onInteract;

        public void TryBeginInteract()
        {
            Interactable interact = interactRay.TryFindInteraction(out Interactable interaction) ? interaction : null;
            onInteract.Invoke(interact);
        }
    }
}
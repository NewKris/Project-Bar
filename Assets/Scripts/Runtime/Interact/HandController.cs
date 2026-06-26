using System;
using Runtime.Player;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact {
    public class HandController : MonoBehaviour {
        public InteractRay interactRay;
        public UnityEvent<HandInteraction> onGrab;
        public UnityEvent<HandInteraction> onRelease;
        public UnityEvent<HandInteraction> onPour;

        public void TryGrabInteract() {
            HandInteraction interact = interactRay.TryFindInteraction(out HandInteraction interaction) ? interaction : null;
            onGrab.Invoke(interact);
        }

        public void TryReleaseInteract() {
            HandInteraction interact = interactRay.TryFindInteraction(out HandInteraction interaction) ? interaction : null;
            onRelease.Invoke(interact);
        }

        public void TryPourInteract() {
            HandInteraction interact = interactRay.TryFindInteraction(out HandInteraction interaction) ? interaction : null;
            onPour.Invoke(interact);
        }
    }
}

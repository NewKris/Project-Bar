using System;
using NaughtyAttributes;
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

        private HandInteraction[] _interactionsBuffer;
        
        public void TryGrabInteract() {
            HandInteraction interact = interactRay.TryFindAnyInteraction(out HandInteraction interaction, _interactionsBuffer) 
                ? interaction 
                : null;
            
            onGrab.Invoke(interact);
        }

        public void TryReleaseInteract() {
            HandInteraction interact = interactRay.TryFindAnyInteraction(out HandInteraction interaction, _interactionsBuffer) 
                ? interaction 
                : null;
            
            onRelease.Invoke(interact);
        }

        public void TryPourInteract() {
            HandInteraction interact = interactRay.TryFindAnyInteraction(out HandInteraction interaction, _interactionsBuffer) 
                ? interaction 
                : null;
            
            onPour.Invoke(interact);
        }

        private void Awake() {
            _interactionsBuffer = new HandInteraction[5];
        }
    }
}

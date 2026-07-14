using System;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Interact {
    public class StationInteractController : MonoBehaviour {
        public InteractRay interactRay;
        
        private StationInteraction _currentInteraction;
        private StationInteraction[] _buffer;

        public void TryBeginInteract() {
            if (interactRay.TryFindAnyInteraction(out StationInteraction interaction, _buffer)) {
                _currentInteraction = interaction;
                interaction.BeginInteraction();
            }
        }

        public void EndInteract() {
            if (_currentInteraction != null) {
                _currentInteraction.EndInteraction();
            }
            
            _currentInteraction = null;
        }

        private void Awake() {
            _buffer = new StationInteraction[5];
        }
    }
}
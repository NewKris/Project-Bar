using System;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Interact {
    public class StationInteractController : MonoBehaviour {
        public InteractRay interactRay;
        
        private StationInteraction _currentInteraction;
        private int _layerMask;

        public void TryBeginInteract() {
            if (interactRay.TryFindInteraction(out StationInteraction interaction, _layerMask)) {
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
            _layerMask = 1 << LayerMask.NameToLayer("Station");
        }
    }
}
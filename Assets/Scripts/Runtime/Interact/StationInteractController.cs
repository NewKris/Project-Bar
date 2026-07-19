using System;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Interact {
    public class StationInteractController : MonoBehaviour {
        public InteractRay interactRay;
        public LayerMask interactMask;
        
        private StationInteraction _currentInteraction;

        public void TryBeginInteract() {
            if (interactRay.TryFindInteraction(out StationInteraction interaction, interactMask)) {
                _currentInteraction = interaction;
                interaction.BeginInteraction();
                
                VerboseDebug.Log($"Started station interaction: {interaction.gameObject.name}");
            }
            else {
                VerboseDebug.Log("No station to interact with");
            }
        }

        public void EndInteract() {
            if (_currentInteraction != null) {
                _currentInteraction.EndInteraction();
                VerboseDebug.Log($"Ended station interaction: {_currentInteraction.gameObject.name}");
            }
            else {
                VerboseDebug.Log("No station currently being used");
            }
            
            _currentInteraction = null;
        }
    }
}
using System;
using System.Collections.Generic;
using Runtime.Interaction;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerInteractController : MonoBehaviour {
        public InteractRay interactRay;
        public ItemDock playerHand;

        private ISpaceInteraction _currentInteraction;
        private List<IHoverInteraction> _currentHovers = new (10);
        private List<IHoverInteraction> _previousHovers = new (10);
        
        private void Awake() {
            PlayerController.OnGrab += TryGrabInteraction;
            PlayerController.OnRelease += TryReleaseOnInteraction;
            PlayerController.OnBeginInteract += TryBeginInteraction;
            PlayerController.OnEndInteract += TryEndInteraction;
        }

        private void OnDestroy() {
            PlayerController.OnGrab -= TryGrabInteraction;
            PlayerController.OnRelease -= TryReleaseOnInteraction;
            PlayerController.OnBeginInteract -= TryBeginInteraction;
            PlayerController.OnEndInteract -= TryEndInteraction;
        }

        private void Update() {
            UpdateHover();
        }

        private void TryGrabInteraction() {
            if (interactRay.TryGetFirstOfType(out IGrabInteraction interaction)) {
                interaction.OnGrabbed();
            }
        }

        private void TryReleaseOnInteraction() {
            if (interactRay.TryGetFirstOfType(out IReleaseOnInteraction<ItemObject> interaction)) {
                interaction.ReleaseOn(playerHand.heldItem);
            }
        }

        private void TryBeginInteraction() {
            if (interactRay.TryGetFirstOfType(out ISpaceInteraction interaction)) {
                _currentInteraction = interaction;
                _currentInteraction.BeginInteraction();
            }
        }
        
        private void TryEndInteraction() {
            if (_currentInteraction != null) {
                _currentInteraction.EndInteraction();
                _currentInteraction = null;
            }
        }

        private void UpdateHover() {
        }
    }
}
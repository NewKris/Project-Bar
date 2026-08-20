using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Interaction;
using Runtime.Items;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerInteractController : MonoBehaviour {
        private const int BUFFER_SIZE = 10;
        
        public InteractRay interactRay;
        public ItemDock playerHand;

        private ISpaceInteraction _currentInteraction;
        private IHoverInteraction[] _currentHovers = new IHoverInteraction[BUFFER_SIZE];
        private IHoverInteraction[] _previousHovers = new IHoverInteraction[BUFFER_SIZE];
        private int _currentHitCount;
        private int _previousHitCount;
        
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
            Array.Fill(_currentHovers, null);
            _currentHitCount = interactRay.GetAllOfTypeNonAlloc(_currentHovers);

            for (int i = 0; i < _currentHitCount; i++) {
                if (!_previousHovers.Contains(_currentHovers[i])) {
                    _currentHovers[i].BeginHover();
                }
            }

            for (int i = 0; i < _previousHitCount; i++) {
                if (!_currentHovers.Contains(_previousHovers[i])) {
                    _previousHovers[i].EndHover();
                }
            }
            
            _previousHitCount = _currentHitCount;
            Array.Copy(_currentHovers, _previousHovers, BUFFER_SIZE);
            
            DebugHits();
        }

        private void DebugHits() {
            for (int i = 0; i < _currentHitCount; i++) {
                Debug.DrawLine(transform.position, _currentHovers[i].GetPosition(), Color.red);
            }
        }
    }
}
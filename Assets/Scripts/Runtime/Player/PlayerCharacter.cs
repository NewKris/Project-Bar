using System;
using Runtime.Interact;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerCharacter : MonoBehaviour {
        public FirstPersonCamera playerCamera;
        public GrabController grabController;
        public PlayerHand hand;
        public InteractController interactController;

        private void Awake() {
            PlayerController.OnGrab += grabController.TryGrabInteract;
            PlayerController.OnRelease += grabController.TryReleaseInteract;
            PlayerController.OnInteract += interactController.TryInteract;
        }

        private void OnDestroy() {
            PlayerController.OnGrab -= grabController.TryGrabInteract;
            PlayerController.OnRelease -= grabController.TryReleaseInteract;
            PlayerController.OnInteract -= interactController.TryInteract;
        }

        private void Update() {
            playerCamera.Look(PlayerController.DeltaMouse, Time.deltaTime);
            hand.Shake = PlayerController.HoldingShake;
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawCapsule(transform.position + Vector3.up, 2, 0.5f, 3, Color.green);
        }
    }
}

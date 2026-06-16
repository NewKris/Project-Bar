using System;
using Runtime.Interact;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerCharacter : MonoBehaviour {
        public FirstPersonCamera playerCamera;
        public InteractController interactController;

        private void Awake() {
            PlayerController.OnGrab += interactController.TryGrabInteract;
            PlayerController.OnRelease += interactController.TryReleaseInteract;
        }

        private void OnDestroy() {
            PlayerController.OnGrab -= interactController.TryGrabInteract;
            PlayerController.OnRelease -= interactController.TryReleaseInteract;
        }

        private void Update() {
            playerCamera.Look(PlayerController.DeltaMouse, Time.deltaTime);
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawCapsule(transform.position + Vector3.up, 2, 0.5f, 3, Color.green);
        }
    }
}

using System;
using Runtime.Interact;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerCharacter : MonoBehaviour {
        public FirstPersonCamera playerCamera;
        public HandController handController;
        public InteractController interactController;
        public StationInteractController stationInteractController;

        private void Awake() {
            PlayerController.OnGrab += handController.TryGrabInteract;
            PlayerController.OnRelease += handController.TryReleaseInteract;
            PlayerController.OnBeginInteract += interactController.TryBeginInteract;
            PlayerController.OnBeginInteract += stationInteractController.TryBeginInteract;
            PlayerController.OnEndInteract += stationInteractController.EndInteract;
            PlayerController.OnPour += handController.TryPourInteract;
            PlayerController.OnCrouch += playerCamera.ChangeCameraHeight;
        }

        private void OnDestroy() {
            PlayerController.OnGrab -= handController.TryGrabInteract;
            PlayerController.OnRelease -= handController.TryReleaseInteract;
            PlayerController.OnBeginInteract -= interactController.TryBeginInteract;
            PlayerController.OnBeginInteract -= stationInteractController.TryBeginInteract;
            PlayerController.OnEndInteract -= stationInteractController.EndInteract;
            PlayerController.OnPour -= handController.TryPourInteract;
            PlayerController.OnCrouch -= playerCamera.ChangeCameraHeight;
        }

        private void Update() {
            playerCamera.Look(PlayerController.DeltaMouse, Time.deltaTime);
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawCapsule(transform.position + Vector3.up, 2, 0.5f, 3, Color.green);
        }
    }
}

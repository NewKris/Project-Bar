using System;
using Runtime.Old_Systems.Interact;
using Runtime.Old_Systems.Player.Hand;
using Runtime.Old_Systems.Stations;
using Runtime.UI;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerCharacter : MonoBehaviour {
        public FirstPersonCamera playerCamera;
        public PlayerHand playerHand;
        public HandController handController;
        public InteractController interactController;
        public StationInteractController stationInteractController;
        public PauseMenu pauseMenu;

        private void Awake() {
            PlayerController.OnGrab += handController.TryGrabInteract;
            PlayerController.OnRelease += handController.TryReleaseInteract;
            PlayerController.OnBeginInteract += interactController.TryBeginInteract;
            PlayerController.OnBeginInteract += stationInteractController.TryBeginInteract;
            PlayerController.OnEndInteract += stationInteractController.EndInteract;
            PlayerController.OnPour += handController.TryPourInteract;
            PlayerController.OnCrouch += playerCamera.ChangeCameraHeight;
            PlayerController.OnBeginShake += playerHand.TryBeginShake;
            PlayerController.OnEndShake += playerHand.TryEndShake;
            PlayerController.TogglePause += pauseMenu.Toggle;
        }

        private void OnDestroy() {
            PlayerController.OnGrab -= handController.TryGrabInteract;
            PlayerController.OnRelease -= handController.TryReleaseInteract;
            PlayerController.OnBeginInteract -= interactController.TryBeginInteract;
            PlayerController.OnBeginInteract -= stationInteractController.TryBeginInteract;
            PlayerController.OnEndInteract -= stationInteractController.EndInteract;
            PlayerController.OnPour -= handController.TryPourInteract;
            PlayerController.OnCrouch -= playerCamera.ChangeCameraHeight;
            PlayerController.OnBeginShake -= playerHand.TryBeginShake;
            PlayerController.OnEndShake -= playerHand.TryEndShake;
            PlayerController.TogglePause -= pauseMenu.Toggle;
        }

        private void Update() {
            if (UIMethods.IsPaused) return;
            
            playerCamera.Look(PlayerController.DeltaMouse, Time.deltaTime);
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawCapsule(transform.position + Vector3.up, 2, 0.5f, 1, Color.green);
        }
    }
}

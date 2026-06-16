using System;
using UnityEngine;

namespace Runtime.Player {
    public class PlayerCharacter : MonoBehaviour {
        public FirstPersonCamera playerCamera;

        private void Update() {
            playerCamera.Look(PlayerController.DeltaMouse, Time.deltaTime);
        }
    }
}

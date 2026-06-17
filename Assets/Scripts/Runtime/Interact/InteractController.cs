using System;
using Runtime.Player;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact {
    public class InteractController : MonoBehaviour {
        public float interactDistance;
        public UnityEvent onRelease;

        public void TryGrabInteract() {
            if (TryFindInteraction(out Interaction interaction)) {
                interaction.GrabInteract();
            }
        }

        public void TryReleaseInteract() {
            if (TryFindInteraction(out Interaction interaction)) {
                interaction.ReleaseInteract();
            }
 
            onRelease.Invoke();
        }

        private bool TryFindInteraction(out Interaction interaction) {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo);

            if (!hit) {
                interaction = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out interaction);
            return interaction != null;
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.red);
        }
    }
}

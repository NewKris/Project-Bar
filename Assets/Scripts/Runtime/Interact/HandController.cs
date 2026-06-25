using System;
using Runtime.Player;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact {
    public class HandController : MonoBehaviour {
        public float interactDistance;
        public UnityEvent<HandInteraction> onGrab;
        public UnityEvent<HandInteraction> onRelease;
        public UnityEvent<HandInteraction> onPour;

        public void TryGrabInteract() {
            HandInteraction interact = TryFindInteraction(out HandInteraction interaction) ? interaction : null;
            onGrab.Invoke(interact);
        }

        public void TryReleaseInteract() {
            HandInteraction interact = TryFindInteraction(out HandInteraction interaction) ? interaction : null;
            onRelease.Invoke(interact);
        }

        public void TryPourInteract() {
            HandInteraction interact = TryFindInteraction(out HandInteraction interaction) ? interaction : null;
            onPour.Invoke(interact);
        }

        private bool TryFindInteraction(out HandInteraction handInteraction) {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo);

            if (!hit) {
                handInteraction = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out handInteraction);
            return handInteraction != null;
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.red);
        }
    }
}

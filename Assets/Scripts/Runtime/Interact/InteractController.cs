using System;
using Runtime.Player;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact {
    public class InteractController : MonoBehaviour {
        public float interactDistance;
        public UnityEvent<Interaction> onGrab;
        public UnityEvent<Interaction> onRelease;

        public void TryGrabInteract() {
            Interaction interact = TryFindInteraction(out Interaction interaction) ? interaction : null;
            onGrab.Invoke(interact);
        }

        public void TryReleaseInteract() {
            Interaction interact = TryFindInteraction(out Interaction interaction) ? interaction : null;
            onRelease.Invoke(interact);
        }

        private bool TryFindInteraction(out Interaction genericGenericInteraction) {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo);

            if (!hit) {
                genericGenericInteraction = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out genericGenericInteraction);
            return genericGenericInteraction != null;
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.red);
        }
    }
}

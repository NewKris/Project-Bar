using System;
using Runtime.Player;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact {
    public class GrabController : MonoBehaviour {
        public float interactDistance;
        public UnityEvent<Grabbable> onGrab;
        public UnityEvent<Grabbable> onRelease;

        public void TryGrabInteract() {
            Grabbable interact = TryFindInteraction(out Grabbable interaction) ? interaction : null;
            onGrab.Invoke(interact);
        }

        public void TryReleaseInteract() {
            Grabbable interact = TryFindInteraction(out Grabbable interaction) ? interaction : null;
            onRelease.Invoke(interact);
        }

        private bool TryFindInteraction(out Grabbable genericGenericGrabbable) {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo);

            if (!hit) {
                genericGenericGrabbable = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out genericGenericGrabbable);
            return genericGenericGrabbable != null;
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.red);
        }
    }
}

using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact {
    public class Interaction : MonoBehaviour {
        public UnityEvent onGrab;
        public UnityEvent onRelease;
        
        public void GrabInteract() {
            Debug.Log("Grab Interact");
            onGrab.Invoke();
        }

        public void ReleaseInteract() {
            Debug.Log("Release Interact");
            onRelease.Invoke();
        }
    }
}
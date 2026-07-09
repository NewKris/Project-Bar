using System;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Looking {
    public class LookController : MonoBehaviour {
        public InteractRay interactRay;

        public LookObject Current { get; private set; }
        
        private void Update() {
            if (interactRay.TryFindInteraction(out LookObject lookObject)) {
                LookAtObject(lookObject);
            } else {
                StopLooking();
            }
        }

        private void StopLooking() {
            if (Current) {
                Current.enabled = false;
                Current = null;
            }
        }

        private void LookAtObject(LookObject lookObject) {
            if (Current && Current != lookObject) {
                Current.enabled = false;
            }
            
            Current = lookObject;
            Current.enabled = true;
        }
    }
}
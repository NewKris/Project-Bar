using System;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Looking {
    public class LookController : MonoBehaviour {
        public InteractRay interactRay;

        private LookObject _current;

        private void Update() {
            if (interactRay.TryFindInteraction(out LookObject lookObject)) {
                LookAtObject(lookObject);
            } else {
                StopLooking();
            }
        }

        private void StopLooking() {
            if (_current) {
                _current.enabled = false;
                _current = null;
            }
        }

        private void LookAtObject(LookObject lookObject) {
            if (_current && _current != lookObject) {
                _current.enabled = false;
            }
            
            _current = lookObject;
            _current.enabled = true;
        }
    }
}
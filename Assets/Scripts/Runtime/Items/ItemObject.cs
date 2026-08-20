using System;
using Runtime.Interaction;
using UnityEngine;

namespace Runtime.Items {
    public class ItemObject : MonoBehaviour, IGrabInteraction {
        public static event Action<ItemObject> OnItemGrabbed;

        private Transform _pin;

        public void Pin(Transform pin) {
            _pin = pin;
            
            if (TryGetComponent(out Rigidbody rb)) {
                rb.isKinematic = true;
            }
            
            SnapToPin();
        }

        public void UnPin() {
            _pin = null;
            
            if (TryGetComponent(out Rigidbody rb)) {
                rb.isKinematic = false;
            }
        }
        
        public void OnGrabbed() {
            OnItemGrabbed?.Invoke(this);
        }

        private void Update() {
            if (_pin) {
                SnapToPin();
            }
        }

        private void SnapToPin() {
            transform.position = _pin.position;
            transform.rotation = _pin.rotation;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Looking {
    public class LookObject : MonoBehaviour {
        public UnityEvent onBeginLook;
        public UnityEvent onEndLook;

        private bool _isLookedAt;
        
        public void LookAt() {
            _isLookedAt = true;
            enabled = true;
        }
        
        private void OnEnable() {
            onBeginLook?.Invoke();
        }

        private void OnDisable() {
            onEndLook?.Invoke();
        }

        private void Awake() {
            enabled = false;
        }

        private void Update() {
            if (_isLookedAt)
                _isLookedAt = false;
            else
                enabled = false;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Looking {
    public class LookObject : MonoBehaviour {
        public UnityEvent onBeginLook;
        public UnityEvent onEndLook;

        private void OnEnable() {
            onBeginLook?.Invoke();
        }

        private void OnDisable() {
            onEndLook?.Invoke();
        }

        private void Awake() {
            enabled = false;
        }
    }
}
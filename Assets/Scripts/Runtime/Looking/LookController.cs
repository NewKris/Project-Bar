using System;
using Runtime.Interact;
using UnityEngine;

namespace Runtime.Looking {
    public class LookController : MonoBehaviour {
        public InteractRay interactRay;
        public int bufferSize = 5;

        private LookObject[] _objectBuffer;

        public LookObject Current => _objectBuffer[0];

        private void Awake() {
            _objectBuffer = new LookObject[bufferSize];
        }

        private void Update() {
            int hitCount = interactRay.TryFindAllInteractions(_objectBuffer);
            for (int i = 0; i < hitCount; i++) {
                _objectBuffer[i].LookAt();
            }
        }
    }
}
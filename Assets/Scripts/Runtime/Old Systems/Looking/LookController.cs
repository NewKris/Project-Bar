using System;
using Runtime.Old_Systems.Interact;
using UnityEngine;

namespace Runtime.Old_Systems.Looking {
    [Obsolete]
    public class LookController : MonoBehaviour {
        public InteractRay interactRay;
        public LayerMask lookLayer;

        private LookObject[] _objectBuffer;

        public LookObject Current => _objectBuffer[0];

        private void Awake() {
            _objectBuffer = new LookObject[interactRay.bufferSize];
        }

        private void Update() {
            int hitCount = interactRay.TryFindAllInteractions(_objectBuffer, lookLayer);
            for (int i = 0; i < hitCount; i++) {
                _objectBuffer[i].LookAt();
            }
        }
    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Animations {
    public class RumbleAnimation : MonoBehaviour {
        public float frequency;
        public float amplitude;

        private bool _shaking;
        private float _lastShake;
        private Vector3 _origin;

        public bool Shaking {
            get => _shaking;
            set{
                _shaking = value;
                if (!_shaking) transform.position = _origin;
            }
        }

        private void Awake() {
            _origin = transform.position;
        }

        private void Update() {
            if (Shaking && Time.time - _lastShake > frequency) {
                transform.position = _origin + Random.insideUnitSphere * amplitude;
                _lastShake = Time.time;
            }
        }
    }
}
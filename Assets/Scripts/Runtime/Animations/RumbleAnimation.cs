using UnityEngine;

namespace Runtime.Animations {
    public class RumbleAnimation : MonoBehaviour {
        public float frequency;
        public float amplitude;

        private bool _shaking;
        private float _lastShake;

        public bool Shaking {
            get => _shaking;
            set{
                _shaking = value;
                if (!_shaking) transform.localPosition = Vector3.zero;
            }
        }
        
        private void Update() {
            if (Shaking && Time.time - _lastShake > frequency) {
                transform.localPosition = Random.insideUnitSphere * amplitude;
                _lastShake = Time.time;
            }
        }
    }
}
using System;
using UnityEngine;

namespace Runtime.Animations {
    public class HandShakeAnimation : MonoBehaviour {
        public float frequency;
        public float amplitude;

        private bool _shake;

        public bool Shake {
            get => _shake;
            set {
                _shake = value;
                
                if (!_shake) transform.localPosition = Vector3.zero;
            }
        }

        private void Update() {
            if (Shake) {
                float offset = Mathf.Sin(Time.time * frequency) * amplitude;
                transform.localPosition = Vector3.up * offset;
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}
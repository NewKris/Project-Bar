using System;
using UnityEngine;

namespace Runtime.Animations {
    public class HandShakeAnimation : MonoBehaviour {
        public float frequency;
        public float amplitude;

        private bool shaking;

        public bool Shaking {
            get => shaking;
            set {
                shaking = value;
                
                if (!shaking) transform.localPosition = Vector3.zero;
            }
        }

        private void Update() {
            if (Shaking) {
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
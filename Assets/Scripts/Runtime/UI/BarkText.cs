using System;
using TMPro;
using UnityEngine;

namespace Runtime.UI {
    public class BarkText : MonoBehaviour {
        public float lifeTime;

        [Header("Floating")]
        public Vector3 floatDirection;
        public AnimationCurve floatCurve;

        [Header("Color")] 
        public Color startColor;
        public Color endColor;
        public AnimationCurve colorCurve;

        private float _spawnTime;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private TMP_Text _text;

        private void OnValidate() {
            if (TryGetComponent(out TMP_Text text)) {
                text.color = startColor;
            }
        }

        private void Start() {
            _startPosition = transform.position;
            _endPosition = transform.position + floatDirection;
            
            _text = GetComponent<TMP_Text>();
            _spawnTime = Time.time;
        }

        private void Update() {
            float t = (Time.time - _spawnTime) / lifeTime;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, floatCurve.Evaluate(t));
            _text.color = Color.Lerp(startColor, endColor, colorCurve.Evaluate(t));

            if (Time.time - _spawnTime > lifeTime) {
                Destroy(gameObject);
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;

            Vector3 p1 = transform.position;
            Vector3 p2 = transform.position + floatDirection;
            
            Gizmos.DrawSphere(p1, 0.05f);
            Gizmos.DrawSphere(p2, 0.05f);
            Gizmos.DrawLine(p1, p2);
        }
    }
}
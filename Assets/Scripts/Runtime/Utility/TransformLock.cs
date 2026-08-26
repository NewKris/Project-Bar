using System;
using UnityEngine;

namespace Runtime.Utility {
    [ExecuteInEditMode]
    public class TransformLock : MonoBehaviour {
        public bool lockPosition;
        public bool lockRotation;
        public bool lockScale;

#if UNITY_EDITOR
        private void Update() {
            if (lockPosition) {
                transform.localPosition = Vector3.zero;
            }

            if (lockRotation) {
                transform.localRotation = Quaternion.identity;
            }

            if (lockScale) {
                transform.localScale = Vector3.one;
            }
        }
#endif
    }
}
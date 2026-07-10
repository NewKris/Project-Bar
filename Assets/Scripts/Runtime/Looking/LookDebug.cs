using System;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookDebug : MonoBehaviour {
        public Material offMaterial;
        public Material onMaterial;

        public void StartLooking() {
            GetComponent<MeshRenderer>().material = onMaterial;
        }

        public void StopLooking() {
            GetComponent<MeshRenderer>().material = offMaterial;
        }

        private void Awake() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(StartLooking);
                lookObject.onEndLook.AddListener(StopLooking);
            }
        }
    }
}
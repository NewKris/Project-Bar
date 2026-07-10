using System;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookDebug : MonoBehaviour {
        public Material offMaterial;
        public Material onMaterial;
        public MeshRenderer[] affectedRenderers;

        public void StartLooking() {
            foreach (MeshRenderer meshRenderer in affectedRenderers) {
                meshRenderer.material = onMaterial;
            }
        }

        public void StopLooking() {
            foreach (MeshRenderer meshRenderer in affectedRenderers) {
                meshRenderer.material = offMaterial;
            }
        }

        private void Reset() {
            affectedRenderers = GetComponentsInChildren<MeshRenderer>(true);
        }

        private void Awake() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(StartLooking);
                lookObject.onEndLook.AddListener(StopLooking);
            }
        }
    }
}
using System;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookMaterial : MonoBehaviour {
        public Material offMaterial;
        public Material onMaterial;
        public MeshRenderer[] affectedRenderers;

        public void StartLooking() {
            foreach (MeshRenderer meshRenderer in affectedRenderers) {
                meshRenderer.material = onMaterial;
            }

            if (TryGetComponent(out MeshRenderer renderer)) {
                renderer.material = onMaterial;
            }
        }

        public void StopLooking() {
            foreach (MeshRenderer meshRenderer in affectedRenderers) {
                meshRenderer.material = offMaterial;
            }
            
            if (TryGetComponent(out MeshRenderer renderer)) {
                renderer.material = offMaterial;
            }
        }

        private void Reset() {
            affectedRenderers = GetComponentsInChildren<MeshRenderer>(true);

            if (TryGetComponent(out MeshRenderer renderer)) {
                offMaterial = renderer.material;
            }
        }

        private void Awake() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(StartLooking);
                lookObject.onEndLook.AddListener(StopLooking);
            }
        }
    }
}
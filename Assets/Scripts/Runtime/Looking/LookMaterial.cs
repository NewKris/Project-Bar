using System;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookMaterial : MonoBehaviour {
        public Material onMaterial;
        public MeshRenderer[] affectedRenderers;

        private Material _offMaterial;

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
                meshRenderer.material = _offMaterial;
            }
            
            if (TryGetComponent(out MeshRenderer renderer)) {
                renderer.material = _offMaterial;
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
            
            if (TryGetComponent(out MeshRenderer renderer)) {
                _offMaterial = renderer.material;
            }
        }
    }
}
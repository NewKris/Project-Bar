using System;
using System.Linq;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookMaterial : MonoBehaviour {
        public Material onMaterial;
        public MeshRenderer[] affectedRenderers;

        private Material _offMaterial;
        private Material[] _offMaterials;

        public void ResetAffectedRenderers() {
            affectedRenderers = GetComponentsInChildren<MeshRenderer>(true);
        }
        
        public void StartLooking() {
            foreach (MeshRenderer meshRenderer in affectedRenderers) {
                meshRenderer.material = onMaterial;
            }

            if (TryGetComponent(out MeshRenderer renderer)) {
                renderer.material = onMaterial;
            }
        }

        public void StopLooking() {
            for (int i = 0; i < affectedRenderers.Length; i++) {
                affectedRenderers[i].material = _offMaterials[i];
            }
            
            if (TryGetComponent(out MeshRenderer renderer)) {
                renderer.material = _offMaterial;
            }
        }

        private void Reset() {
            ResetAffectedRenderers();
        }

        private void Awake() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(StartLooking);
                lookObject.onEndLook.AddListener(StopLooking);
            }
            
            if (TryGetComponent(out MeshRenderer renderer)) {
                _offMaterial = renderer.material;
            }
            
            _offMaterials = affectedRenderers.Select(x => x.material).ToArray();
        }
    }
}
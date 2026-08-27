using System;
using System.Linq;
using UnityEngine;

namespace Runtime.Old_Systems.Looking {
    [Obsolete]
    [RequireComponent(typeof(LookObject))]
    public class LookMaterial : MonoBehaviour {
        public Material onMaterial;
        public MeshRenderer[] affectedRenderers;

        private bool _flashed;
        private Material _offMaterial;
        private Material[] _offMaterials;

        public void ResetAffectedRenderers() {
            affectedRenderers = GetComponentsInChildren<MeshRenderer>(true);
        }

        public void StartFlash() {
            _flashed = true;
            SetOnMaterial();
        }

        public void EndFlash() {
            _flashed = false;
            SetOffMaterial();
        }
        
        public void StartLooking() {
            if (_flashed) return;
            
            SetOnMaterial();
        }

        public void StopLooking() {
            if (_flashed) return;
            
            SetOffMaterial();
        }

        private void SetOffMaterial() {
            for (int i = 0; i < affectedRenderers.Length; i++) {
                affectedRenderers[i].material = _offMaterials[i];
            }
            
            if (TryGetComponent(out MeshRenderer renderer)) {
                renderer.material = _offMaterial;
            }
        }
        
        private void SetOnMaterial() {
            foreach (MeshRenderer meshRenderer in affectedRenderers) {
                meshRenderer.material = onMaterial;
            }

            if (TryGetComponent(out MeshRenderer renderer)) {
                renderer.material = onMaterial;
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
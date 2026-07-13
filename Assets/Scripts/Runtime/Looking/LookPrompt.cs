using System;
using NaughtyAttributes;
using Runtime.Utility;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookPrompt : MonoBehaviour {
        public string promptText;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;

        [Foldout("Debug")] public float debugSize = 0.2f;
        [Foldout("Debug")] public Color debugColor = Color.green;
        
        private PromptText _prompt;

        public void ShowPrompt() {
            _prompt.gameObject.SetActive(true);
        }

        public void HidePrompt() {
            _prompt.gameObject.SetActive(false);
        }
        
        private void Awake() {
            PromptCanvas canvas = FindAnyObjectByType<PromptCanvas>();
            
            Vector3 pos = transform.position + positionOffset;
            Quaternion rot = transform.rotation * Quaternion.Euler(rotationOffset);
            
            _prompt = canvas.CreatePromptText(promptText, pos, rot);
            
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(_prompt.HighlightPrompt);
                lookObject.onEndLook.AddListener(_prompt.StopHighlight);
            }
        }

        private void OnDrawGizmos() {
            Vector3 pos = transform.position + positionOffset;
            Vector3 forward = Quaternion.Euler(rotationOffset) * transform.forward;
            HandlesProxy.DrawDisc(pos, forward, debugSize, false, debugColor);
        }
    }
}
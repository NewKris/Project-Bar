using System;
using NaughtyAttributes;
using Runtime.Utility;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookPrompt : MonoBehaviour {
        public string promptText;
        public Transform promptTransform;
        public PromptCanvas promptCanvas;

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
            Transform pivot = promptTransform != null ? promptTransform : transform;
            _prompt = promptCanvas.CreatePromptText(promptText, pivot);
            
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(_prompt.HighlightPrompt);
                lookObject.onEndLook.AddListener(_prompt.StopHighlight);
            }
        }

        private void OnDrawGizmos() {
            Transform pivot = promptTransform != null ? promptTransform : transform;
            HandlesProxy.DrawDisc(pivot.position, pivot.forward, debugSize, false, debugColor);
        }
    }
}
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

        [Foldout("Debug")] public float debugSize = 0.05f;
        [Foldout("Debug")] public Color debugColor = new Color(0f, 1f, 0f, 0.2f);
        
        private PromptText _prompt;
        
        private Vector3 PromptPosition => transform.position + transform.TransformDirection(positionOffset);
        private Quaternion PromptRotation => transform.rotation * Quaternion.Euler(rotationOffset);

        public void ShowPrompt() {
            _prompt.gameObject.SetActive(true);
        }

        public void HidePrompt() {
            _prompt.gameObject.SetActive(false);
        }
        
        private void Awake() {
            PromptCanvas canvas = FindAnyObjectByType<PromptCanvas>();

            _prompt = canvas.CreatePromptText(promptText, PromptPosition, PromptRotation);
            
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(_prompt.HighlightPrompt);
                lookObject.onEndLook.AddListener(_prompt.StopHighlight);
            }
        }

        private void OnDrawGizmos() {
            Vector3 forward = PromptRotation * Vector3.forward;
            HandlesProxy.DrawDisc(PromptPosition, forward, debugSize, false, debugColor);
        }
    }
}
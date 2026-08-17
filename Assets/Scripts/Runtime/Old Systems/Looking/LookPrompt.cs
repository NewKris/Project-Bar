using System;
using NaughtyAttributes;
using Runtime.UI;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Old_Systems.Looking {
    [Obsolete]
    [RequireComponent(typeof(LookObject))]
    public class LookPrompt : MonoBehaviour {
        public string promptText;
        public Transform pivot;

        [Foldout("Debug")] public float debugSize = 0.05f;
        [Foldout("Debug")] public Color debugColor = new Color(0f, 1f, 0f, 0.2f);
        
        private PromptText _prompt;
        
        private Vector3 PromptPosition => pivot ? pivot.position : transform.position;
        private Quaternion PromptRotation => pivot ? pivot.rotation : transform.rotation;

        public void ShowPrompt() {
            _prompt.gameObject.SetActive(true);
        }

        public void HidePrompt() {
            _prompt.gameObject.SetActive(false);
        }
        
        private void Awake() {
            WorldSpaceCanvas canvas = FindAnyObjectByType<WorldSpaceCanvas>();

            _prompt = canvas.CreatePromptText(promptText, PromptPosition, PromptRotation);
            
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(_prompt.HighlightPrompt);
                lookObject.onEndLook.AddListener(_prompt.StopHighlight);
            }
        }

        private void OnDrawGizmos() {
            Vector3 forward = PromptRotation * Vector3.back;
            Vector3 up = PromptRotation * Vector3.up;
            HandlesProxy.DrawDisc(PromptPosition, forward, debugSize, false, debugColor);
            
            Gizmos.color = Color.red;
            Gizmos.DrawRay(PromptPosition, forward * 0.1f);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(PromptPosition, up * 0.1f);
        }
    }
}
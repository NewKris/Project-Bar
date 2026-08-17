using System;
using Runtime.Old_Systems.Interact;
using UnityEngine;

namespace Runtime.Old_Systems.Looking {
    [Obsolete]
    [RequireComponent(typeof(LookObject))]
    public class PromptArea : MonoBehaviour {
        private void Reset() {
            if (!TryGetComponent(out Collider _)) {
                BoxCollider col = gameObject.AddComponent<BoxCollider>();
                col.isTrigger = true;
            }
        }

        private void Start() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(EnableAllPrompts);
                lookObject.onEndLook.AddListener(DisableAllPrompts);
            }
            
            TryAssignLayer();
        }

        private void OnDestroy() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.RemoveListener(EnableAllPrompts);
                lookObject.onEndLook.RemoveListener(DisableAllPrompts);
            }
        }

        private void EnableAllPrompts() {
            foreach (LookPrompt prompt in GetComponentsInChildren<LookPrompt>()) {
                prompt.ShowPrompt();
            }
        }

        private void DisableAllPrompts() {
            foreach (LookPrompt prompt in GetComponentsInChildren<LookPrompt>()) {
                prompt.HidePrompt();
            }
        }

        private void TryAssignLayer() {
            if (!TryGetComponent(out IInteraction _)) {
                gameObject.layer = LayerMask.NameToLayer("Ignore Interactions");
            }
        }
    }
}
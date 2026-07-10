using System;
using UnityEngine;

namespace Runtime.Looking {
    public class PromptArea : MonoBehaviour {
        private void Awake() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(EnableAllPrompts);
                lookObject.onEndLook.AddListener(DisableAllPrompts);
            }
        }

        private void EnableAllPrompts() {
            foreach (PromptText prompt in GetComponentsInChildren<PromptText>(true)) {
                prompt.gameObject.SetActive(true);
            }
        }

        private void DisableAllPrompts() {
            foreach (PromptText prompt in GetComponentsInChildren<PromptText>(true)) {
                prompt.gameObject.SetActive(false);
            }
        }
    }
}
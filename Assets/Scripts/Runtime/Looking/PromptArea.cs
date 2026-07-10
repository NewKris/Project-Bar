using System;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject), typeof(BoxCollider))]
    public class PromptArea : MonoBehaviour {
        private void Reset() {
            GetComponent<BoxCollider>().isTrigger = true;
        }

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
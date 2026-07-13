using System;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject), typeof(BoxCollider))]
    public class PromptArea : MonoBehaviour {
        private void Reset() {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void Start() {
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(EnableAllPrompts);
                lookObject.onEndLook.AddListener(DisableAllPrompts);
            }
        }

        private void EnableAllPrompts() {
            foreach (LookPrompt prompt in GetComponentsInChildren<LookPrompt>(true)) {
                prompt.ShowPrompt();
            }
        }

        private void DisableAllPrompts() {
            foreach (LookPrompt prompt in GetComponentsInChildren<LookPrompt>(true)) {
                prompt.HidePrompt();
            }
        }
    }
}
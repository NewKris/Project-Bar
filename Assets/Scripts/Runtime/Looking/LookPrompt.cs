using System;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    [RequireComponent(typeof(LookObject))]
    public class LookPrompt : MonoBehaviour {
        public string promptText;
        
        private void Awake() {
            PromptText prompt = InitializePrompt();
            
            if (!prompt) return;
            
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(prompt.HighlightPrompt);
                lookObject.onEndLook.AddListener(prompt.StopHighlight);
            }
        }

        private PromptText InitializePrompt() {
            PromptText prompt = GetComponentInChildren<PromptText>(true);

            if (prompt != null) {
                prompt.GetComponent<TextMeshProUGUI>().text = promptText;
                prompt.gameObject.SetActive(false);
            }

            return prompt;
        }
    }
}
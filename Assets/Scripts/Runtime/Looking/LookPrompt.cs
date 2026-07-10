using System;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    public class LookPrompt : MonoBehaviour {
        public string promptText;
        
        private void Awake() {
            PromptText prompt = InitializePrompt();
            
            if (TryGetComponent(out LookObject lookObject)) {
                lookObject.onBeginLook.AddListener(prompt.HighlightPrompt);
                lookObject.onEndLook.AddListener(prompt.StopHighlight);
            }
        }

        private PromptText InitializePrompt() {
            PromptText prompt = GetComponentInChildren<PromptText>(true);
            prompt.GetComponent<TextMeshProUGUI>().text = $"[{promptText}]";
            prompt.gameObject.SetActive(false);

            return prompt;
        }
    }
}
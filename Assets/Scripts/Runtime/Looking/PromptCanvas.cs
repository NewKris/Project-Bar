using UnityEngine;

namespace Runtime.Looking {
    public class PromptCanvas : MonoBehaviour {
        private static PromptCanvas Instance;
        
        public GameObject promptTextPrefab;

        public PromptText CreatePromptText(string text, Transform pivot) {
            PromptText promptText = Instantiate(promptTextPrefab, transform).GetComponent<PromptText>();
            promptText.Initialize(text, pivot);

            return promptText;
        }
    }
}
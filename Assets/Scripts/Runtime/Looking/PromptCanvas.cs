using UnityEngine;

namespace Runtime.Looking {
    public class PromptCanvas : MonoBehaviour {
        private static PromptCanvas Instance;
        
        public GameObject promptTextPrefab;

        public PromptText CreatePromptText(string text, Vector3 position, Quaternion rotation) {
            PromptText promptText = Instantiate(promptTextPrefab, transform).GetComponent<PromptText>();
            promptText.Initialize(text, position, rotation);

            return promptText;
        }
    }
}
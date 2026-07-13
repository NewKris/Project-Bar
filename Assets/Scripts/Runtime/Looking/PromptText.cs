using System;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    public class PromptText : MonoBehaviour {
        public Color highlightColor;

        public void Initialize(string text, Transform pivot) {
            GetComponent<TextMeshProUGUI>().text = text;
            transform.position = pivot.position;
            transform.rotation = pivot.rotation;
            gameObject.SetActive(false);
        }
        
        public void HighlightPrompt() {
            GetComponent<TextMeshProUGUI>().color = highlightColor;
        }

        public void StopHighlight() {
            GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }
}
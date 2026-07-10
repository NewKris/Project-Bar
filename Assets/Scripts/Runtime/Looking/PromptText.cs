using System;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    public class PromptText : MonoBehaviour {
        public Color highlightColor;
        
        public void HighlightPrompt() {
            GetComponent<TextMeshProUGUI>().color = highlightColor;
        }

        public void StopHighlight() {
            GetComponent<TextMeshProUGUI>().color = Color.white;
        }
    }
}
using System;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    public class InteractPrompt : MonoBehaviour {
        public string inputKey;
        public Color highlightColor;
        
        public void HighlightPrompt() {
            GetComponent<TextMeshProUGUI>().color = highlightColor;
        }

        public void StopHighlight() {
            GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        private void Awake() {
            GetComponent<TextMeshProUGUI>().text = $"[{inputKey}]";
        }
    }
}
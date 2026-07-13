using System;
using TMPro;
using UnityEngine;

namespace Runtime.Looking {
    public class PromptText : MonoBehaviour {
        public Color highlightColor;

        public void Initialize(string text, Vector3 position, Quaternion rotation) {
            GetComponent<TextMeshProUGUI>().text = text;
            transform.position = position;
            transform.rotation = rotation;
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
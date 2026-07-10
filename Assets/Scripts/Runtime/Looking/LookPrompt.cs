using System;
using UnityEngine;

namespace Runtime.Looking {
    public class LookPrompt : MonoBehaviour {
        private void Awake() {
            if (TryGetComponent(out LookObject lookObject)) {
                InteractPrompt prompt = GetComponentInChildren<InteractPrompt>();
                lookObject.onBeginLook.AddListener(prompt.HighlightPrompt);
                lookObject.onEndLook.AddListener(prompt.StopHighlight);
            }
        }
    }
}
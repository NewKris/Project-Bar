using NaughtyAttributes;
using Runtime.Looking;
using UnityEngine;

namespace Runtime {
    public class SetupWizard : MonoBehaviour {
        public Color promptColor;
        public float promptSize;
        
        [Button]
        public void ResetPromptDebugColors() {
            foreach (LookPrompt prompt in FindObjectsByType<LookPrompt>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
                prompt.debugColor  = promptColor;
            }
        }
        
        [Button]
        public void ResetPromptDebugSize() {
            foreach (LookPrompt prompt in FindObjectsByType<LookPrompt>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
                prompt.debugSize  = promptSize;
            }
        }
    }
}
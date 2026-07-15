using NaughtyAttributes;
using Runtime.Looking;
using UnityEngine;

namespace Runtime {
    public class SetupWizard : MonoBehaviour {
        public Color promptColor;
        
        [Button]
        public void ResetPromptColors() {
            foreach (LookPrompt prompt in FindObjectsByType<LookPrompt>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
                prompt.debugColor  = promptColor;
            }

            
        }
    }
}
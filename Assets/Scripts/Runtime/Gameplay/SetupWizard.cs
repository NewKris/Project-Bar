using NaughtyAttributes;
using Runtime.Interact;
using Runtime.Looking;
using Runtime.Stations;
using Runtime.Utility.Extensions;
using UnityEngine;

namespace Runtime.Gameplay {
    public class SetupWizard : MonoBehaviour {
        public Color promptColor;
        public float promptSize;

        [Button]
        public void ResetLookMaterials() {
            FindObjectsByType<LookMaterial>(FindObjectsSortMode.None).ForEach(x => x.ResetAffectedRenderers());
        }
        
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

        [Button]
        public void SetAllStationLayers() {
            foreach (Station station in FindObjectsByType<Station>(FindObjectsInactive.Include, FindObjectsSortMode.None)) {
                station.gameObject.layer = LayerMask.NameToLayer("Station");
            }
        }
    }
}
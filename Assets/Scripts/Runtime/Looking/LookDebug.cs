using UnityEngine;

namespace Runtime.Looking {
    public class LookDebug : MonoBehaviour {
        public Material offMaterial;
        public Material onMaterial;

        public void StartLooking() {
            GetComponent<MeshRenderer>().material = onMaterial;
        }

        public void StopLooking() {
            GetComponent<MeshRenderer>().material = offMaterial;
        }
    }
}
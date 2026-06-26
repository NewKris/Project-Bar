using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Interact {
    public class StationInteraction : MonoBehaviour {
        public UnityEvent onBeginInteraction;
        public UnityEvent onEndInteraction;
        
        public void BeginInteraction() {
            onBeginInteraction.Invoke();
        }

        public void EndInteraction() {
            onEndInteraction.Invoke();
        }
    }
}
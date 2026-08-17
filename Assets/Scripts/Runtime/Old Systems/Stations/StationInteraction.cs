using System;
using Runtime.Old_Systems.Interact;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Old_Systems.Stations {
    [Obsolete]
    public class StationInteraction : MonoBehaviour, IInteraction {
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
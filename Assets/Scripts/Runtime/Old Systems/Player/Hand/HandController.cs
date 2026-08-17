using System;
using Runtime.Old_Systems.Interact;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Old_Systems.Player.Hand {
    [Obsolete]
    public class HandController : MonoBehaviour {
        public InteractRay interactRay;
        public UnityEvent<HandInteraction> onGrab;
        public UnityEvent<HandInteraction> onRelease;
        public UnityEvent<HandInteraction> onPour;

        public void TryGrabInteract() {
            HandInteraction interact = interactRay.TryFindInteraction(out HandInteraction interaction) 
                ? interaction 
                : null;
            
            VerboseDebug.Log($"Grabbed on: {interact?.gameObject.name ?? "Nothing"}");
            
            onGrab.Invoke(interact);
        }

        public void TryReleaseInteract() {
            HandInteraction interact = interactRay.TryFindInteraction(out HandInteraction interaction) 
                ? interaction 
                : null;
            
            VerboseDebug.Log($"Released on: {interact?.gameObject.name ?? "Nothing"}");
            
            onRelease.Invoke(interact);
        }

        public void TryPourInteract() {
            HandInteraction interact = interactRay.TryFindInteraction(out HandInteraction interaction) 
                ? interaction 
                : null;
            
            VerboseDebug.Log($"Poured on: {interact?.gameObject.name ?? "Nothing"}");
            
            onPour.Invoke(interact);
        }
    }
}

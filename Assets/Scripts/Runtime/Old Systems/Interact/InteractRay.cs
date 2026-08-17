using System;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Old_Systems.Interact {
    [Obsolete]
    public class InteractRay : MonoBehaviour {
        public float interactDistance;
        public LayerMask defaultMask;
        public int bufferSize = 5;

        private RaycastHit[] _hitBuffer;
        
        public bool TryFindInteraction<T>(out T interactable) where T: MonoBehaviour {
            return TryFindInteraction(out interactable, defaultMask);
        }
        
        public bool TryFindInteraction<T>(out T interactable, LayerMask mask) where T : MonoBehaviour {
            interactable = null;

            Ray ray =  new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, mask);

            return hitInfo.collider?.TryGetComponent(out interactable) ?? false;
        }
        
        public int TryFindAllInteractions<T>(T[] interactables) where T: MonoBehaviour {
            return TryFindAllInteractions(interactables, defaultMask);
        }
        
        public int TryFindAllInteractions<T>(T[] interactables, LayerMask layerMask) where T: MonoBehaviour {
            Ray ray =  new Ray(transform.position, transform.forward);
            int hitCount =  Physics.RaycastNonAlloc(ray, _hitBuffer, interactDistance, layerMask);

            int interactCount = 0;
            
            for (int i = 0 ; i < hitCount; i++) {
                if (_hitBuffer[i].collider.TryGetComponent(out T interaction)) {
                    interactables[interactCount] = interaction;
                    interactCount++;
                }
            }
            
            return interactCount;
        }

        private void Awake() {
            _hitBuffer =  new RaycastHit[bufferSize];
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.magenta);
        }
    }
}
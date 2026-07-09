using System;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Interact {
    public class InteractRay : MonoBehaviour {
        public float interactDistance;
        public LayerMask defaultMask;
        public int bufferSize = 5;

        private RaycastHit[] _hitBuffer;
        
        public bool TryFindInteraction<T>(out T genericInteractable) where T: MonoBehaviour {
            return TryFindInteraction(out genericInteractable, defaultMask);
        }
        
        public bool TryFindInteraction<T>(out T genericInteractable, int layerMask) where T: MonoBehaviour {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, layerMask);

            if (!hit) {
                genericInteractable = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out genericInteractable);
            return genericInteractable != null;
        }
        
        public int TryFindAllInteractions<T>(T[] interactables) where T: MonoBehaviour {
            return TryFindAllInteractions(interactables, defaultMask);
        }
        
        public int TryFindAllInteractions<T>(T[] interactables, int layerMask) where T: MonoBehaviour {
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
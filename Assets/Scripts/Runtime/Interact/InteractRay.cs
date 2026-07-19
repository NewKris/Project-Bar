using System;
using System.Linq;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Interact {
    public class InteractRay : MonoBehaviour {
        public float interactDistance;
        public LayerMask defaultMask;
        public int bufferSize = 5;

        private RaycastHit[] _hitBuffer;
        
        public bool TryFindInteraction<T>(out T interactables) where T: MonoBehaviour {
            return TryFindInteraction(out interactables, defaultMask);
        }
        
        public bool TryFindInteraction<T>(out T interactables, int layerMask) where T: MonoBehaviour {
            Ray ray =  new Ray(transform.position, transform.forward);
            bool hit =  Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, layerMask);

            if (!hit) {
                interactables = null;
                return false;
            }
            
            hitInfo.collider.TryGetComponent(out interactables);
            return interactables != null;
        }

        public bool TryFindAnyInteraction<T>(out T interactables) where T : MonoBehaviour {
            Ray ray =  new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, defaultMask);

            return hitInfo.collider.TryGetComponent(out interactables);
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
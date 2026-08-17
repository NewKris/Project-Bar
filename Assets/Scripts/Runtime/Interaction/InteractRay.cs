using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Utility;
using Runtime.Utility.Extensions;
using UnityEngine;

namespace Runtime.Interaction {
    public class InteractRay : MonoBehaviour {
        public float interactDistance;
        public LayerMask interactMask;
        public int bufferSize = 10;

        private RaycastHit[] _hitBuffer;

        public T GetFirstOfType<T>() where T : IInteraction {
            return _hitBuffer
                .ToList()
                .Sorted(CompareDistance)
                .First(x => x.collider.TryGetComponent(out T _))
                .collider.GetComponent<T>();
        }

        public List<T> GetAllOfType<T>() where T : IInteraction {
            return _hitBuffer
                .Where(x => x.collider.TryGetComponent(out T _))
                .Select(x => x.collider.GetComponent<T>())
                .ToList();
        }
        
        private void Awake() {
            _hitBuffer =  new RaycastHit[bufferSize];
        }

        private void Update() {
            Ray ray = new Ray(transform.position, transform.forward);
            Physics.RaycastNonAlloc(ray, _hitBuffer, interactDistance, interactMask);
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.magenta);
        }

        private int CompareDistance(RaycastHit a, RaycastHit b) {
            float d1 = (a.transform.position - transform.position).sqrMagnitude;
            float d2 = (b.transform.position - transform.position).sqrMagnitude;
            
            return d1  < d2 ? -1 : 1;
        }
        
        private int CompareDistance(IInteraction a, IInteraction b) {
            float d1 = (a.GetPosition() - transform.position).sqrMagnitude;
            float d2 = (b.GetPosition() - transform.position).sqrMagnitude;
            
            return d1  < d2 ? -1 : 1;
        }
    }
}
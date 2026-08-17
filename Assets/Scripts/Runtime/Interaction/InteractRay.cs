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

        private int _hitCount;
        private RaycastHit[] _hitBuffer;

        public bool TryGetFirstOfType<T>(out T hit) where T : IInteraction {
            hit = default(T);
            return false;
        }

        public int GetAllOfTypeNonAlloc<T>(T[] buffer) where T : IInteraction {
            return 0;
        }
        
        private void Awake() {
            _hitBuffer =  new RaycastHit[bufferSize];
        }

        private void Update() {
            Ray ray = new Ray(transform.position, transform.forward);
            _hitCount = Physics.RaycastNonAlloc(ray, _hitBuffer, interactDistance, interactMask);
        }

        private void OnDrawGizmos() {
            HandlesProxy.DrawLine(transform.position, transform.position + transform.forward * interactDistance, 3, true, Color.magenta);
        }
        
        private int CompareDistance<T>(T a, T b) where T : IInteraction {
            float d1 = (a.GetPosition() - transform.position).sqrMagnitude;
            float d2 = (b.GetPosition() - transform.position).sqrMagnitude;
            
            return d1  < d2 ? -1 : 1;
        }
    }
}
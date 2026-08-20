using System;
using System.Collections.Generic;
using System.Linq;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Interaction {
    public class InteractRay : MonoBehaviour {
        public float interactDistance;
        public LayerMask interactMask;
        public int bufferSize = 10;

        private int _hitCount;
        private RaycastHit[] _hitBuffer;
        private List<IInteraction> _interactBuffer;

        public bool TryGetFirstOfType<T>(out T interaction) where T : IInteraction {
            interaction = (T)_interactBuffer.FirstOrDefault(x => x is T);
            return interaction != null;
        }

        public int GetAllOfTypeNonAlloc<T>(T[] buffer) where T : IInteraction {
            int findCount = 0;
            
            foreach (IInteraction interaction in _interactBuffer) {
                if (interaction is T eligibleInteraction) {
                    buffer[findCount] = eligibleInteraction;
                    findCount++;
                }
            }
            
            return findCount;
        }
        
        private void Awake() {
            _hitBuffer =  new RaycastHit[bufferSize];
            _interactBuffer = new List<IInteraction>(bufferSize);
        }

        private void Update() {
            Ray ray = new Ray(transform.position, transform.forward);
            _hitCount = Physics.RaycastNonAlloc(ray, _hitBuffer, interactDistance, interactMask);
            _interactBuffer.Clear();

            for (int i = 0; i < _hitCount; i++) {
                if (_hitBuffer[i].collider.TryGetComponent(out IInteraction interaction)) {
                    _interactBuffer.Add(interaction);
                }
            }
            
            _interactBuffer.Sort(CompareDistance);
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
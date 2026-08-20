using UnityEngine;

namespace Runtime.Interaction {
    public interface IInteraction {
        /*public bool TryGetComponent<T>(out T componentOut) where T : Component {
            if (this is Component component) {
                return component.TryGetComponent(out componentOut);
            }
            else {
                componentOut = null;
                return false;
            }
        }*/
        
        public Vector3 GetPosition() {
            if (this is Component component) {
                return component.transform.position;
            }
            else {
                return Vector3.zero;
            }
        }
    }
}
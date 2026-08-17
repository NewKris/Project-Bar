using UnityEngine;

namespace Runtime.Interaction {
    public interface IInteraction {
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
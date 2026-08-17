using UnityEngine;

namespace Runtime.Interaction {
    public interface IReleaseOnInteraction<in T> : IInteraction where T : Component {
        public void ReleaseOn(T releasedObject);
    }
}
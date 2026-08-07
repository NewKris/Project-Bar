using FMODUnity;
using UnityEngine;

namespace Runtime.Audio {
    public struct OneShotConfig {
        public EventReference eventReference;
        public Vector3 position;
        public GameObject attachedGameObject;
        public FmodParameter[] parameters;
    }
}
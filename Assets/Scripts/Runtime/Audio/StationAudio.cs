using FMODUnity;
using UnityEngine;

namespace Runtime.Audio {
    [CreateAssetMenu(menuName = "Audio/Station Audio")]
    public class StationAudio : ScriptableObject {
        public EventReference activeLoop;
    }
}
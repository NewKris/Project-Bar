using FMODUnity;
using UnityEngine;

namespace Runtime.Audio {
    [CreateAssetMenu(menuName = "Audio/Item Audio")]
    public class ItemAudio : ScriptableObject {
        public EventReference pickUpAudio;
        public EventReference putDownAudio;
        public EventReference pourAudio;
        public EventReference breakAudio;
    }
}
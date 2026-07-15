using System;
using FMODUnity;
using UnityEngine;

namespace Runtime.Audio {
    public class MusicSource : MonoBehaviour {
        public EventReference music;
        public bool playOnStart = true;

        private void Start() {
            if (playOnStart) {
                MusicManager.PlayMusic(music);
            }
        }
    }
}
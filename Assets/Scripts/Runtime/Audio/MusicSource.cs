using System;
using FMODUnity;
using UnityEngine;

namespace Runtime.Audio {
    public class MusicSource : MonoBehaviour {
        public EventReference music;

        private void OnEnable() {
            MusicManager.PlayMusic(music);
        }
    }
}
using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Audio {
    public class MusicManager : MonoBehaviour {
        private static MusicManager Instance;
        private static EventReference CurrentMusic;

        public static void PlayMusic(EventReference music) {
            
        }

        private void Awake() {
            if (Singleton.SetSingleton(ref Instance, this)) {
                
            }
        }

        private void OnDestroy() {
            Singleton.SetSingleton(ref Instance, this);
        }
    }
}

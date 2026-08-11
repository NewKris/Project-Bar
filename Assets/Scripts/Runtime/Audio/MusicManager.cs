using System;
using System.Collections;
using FMOD.Studio;
using FMODUnity;
using Runtime.Utility;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Runtime.Audio {
    public class MusicManager : MonoBehaviour {
        private static MusicManager Instance;
        private static EventInstance CurrentMusic;

        public static EventInstance PlayMusic(EventReference music) {
            if (CurrentMusic.isValid()) {
                CurrentMusic.stop(STOP_MODE.ALLOWFADEOUT);
            }

            CurrentMusic = RuntimeManager.CreateInstance(music);
            CurrentMusic.start();

            return CurrentMusic;
        }

        private void Awake() {
            if (Singleton.SetSingleton(ref Instance, this)) {
                
            }
        }

        private void OnDestroy() {
            if (Singleton.SetSingleton(ref Instance, this) && CurrentMusic.isValid()) {
                CurrentMusic.stop(STOP_MODE.IMMEDIATE);
            }
        }
        
    }
}

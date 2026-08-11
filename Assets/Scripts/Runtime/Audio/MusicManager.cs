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

        public static void PlayMusic(EventReference music) {
            if (CurrentMusic.isValid()) {
                CurrentMusic.stop(STOP_MODE.ALLOWFADEOUT);
            }

            CurrentMusic = RuntimeManager.CreateInstance(music);
            CurrentMusic.start();
        }

        public static void SetParameter(string parameterName, float value) {
            Debug.Log($"Set FMOD parameter {parameterName} to {value}");
            CurrentMusic.setParameterByName(parameterName, value);
        }

        public static void SetParameter(string parameterName, string value) {
            Debug.Log($"Set FMOD parameter {parameterName} to {value}");
            CurrentMusic.setParameterByNameWithLabel(parameterName, value);
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

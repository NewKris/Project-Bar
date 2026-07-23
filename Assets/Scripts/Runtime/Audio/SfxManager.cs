using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Runtime.Utility;
using UnityEngine;
using Debug = UnityEngine.Debug;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Runtime.Audio {
    public class SfxManager : MonoBehaviour {
        private static readonly Dictionary<string, EventInstance> ActiveAudio = new  Dictionary<string, EventInstance>();

        public static string CreateUniqueKey(MonoBehaviour instance, EventReference audio, int id = 0) {
            return instance.GetInstanceID() + audio.Path + id;
        }
        
        public static void StartAudio(string key, EventReference audio) {
            if (!ActiveAudio.ContainsKey(key) && !audio.IsNull) {
                Debug.Log("Start");
                EventInstance instance = RuntimeManager.CreateInstance(audio);
                ActiveAudio.Add(key, instance);
                
                instance.start();
            }
        }

        public static void StopAudio(string key) {
            if (ActiveAudio.ContainsKey(key)) {
                Debug.Log("Stop");
                
                ActiveAudio[key].stop(STOP_MODE.IMMEDIATE);
                ActiveAudio[key].release();
                ActiveAudio.Remove(key);
            }
        }
        
        public static void PlayOneShot(EventReference audio) {
            if (audio.IsNull) return;
            
            VerboseDebug.Log($"Playing oneshot: {audio}");
            RuntimeManager.PlayOneShot(audio);
        }
        
        public static void PlayOneShot(EventReference audio, Vector3 position) {
            if (audio.IsNull) return;

            VerboseDebug.Log($"Playing oneshot: {audio} at {position}");
            RuntimeManager.PlayOneShot(audio, position);
        }
        
        public static void PlayOneShot(EventReference audio, GameObject gameObject) {
            if (audio.IsNull) return;
            
            VerboseDebug.Log($"Playing oneshot: {audio} on {gameObject.name}");
            RuntimeManager.PlayOneShotAttached(audio, gameObject);
        }

        private void OnDestroy() {
            StopAllAudio();
        }

        private void StopAllAudio() {
            foreach (EventInstance instance in ActiveAudio.Values) {
                instance.stop(STOP_MODE.IMMEDIATE);
            }
            
            ActiveAudio.Clear();
        }
    }
}
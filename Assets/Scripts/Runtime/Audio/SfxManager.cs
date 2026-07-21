using FMODUnity;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Audio {
    public static class SfxManager {
        public static void PlayOneShot(EventReference audio) {
            VerboseDebug.Log($"Playing oneshot: {audio}");
            RuntimeManager.PlayOneShot(audio);
        }
        
        public static void PlayOneShot(EventReference audio, Vector3 position) {
            VerboseDebug.Log($"Playing oneshot: {audio} at {position}");
            RuntimeManager.PlayOneShot(audio, position);
        }
        
        public static void PlayOneShot(EventReference audio, GameObject gameObject) {
            VerboseDebug.Log($"Playing oneshot: {audio} on {gameObject.name}");
            RuntimeManager.PlayOneShotAttached(audio, gameObject);
        }
    }
}
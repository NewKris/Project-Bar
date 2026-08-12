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
        private static SfxManager Instance;
        private static readonly Dictionary<string, EventInstance> ActiveAudio = new  Dictionary<string, EventInstance>();

        public EventReference jingleEvent;

        public static string CreateUniqueKey(MonoBehaviour instance, EventReference audio, int id = 0) {
            return instance.GetInstanceID() + audio.ToString() + id;
        }
        
        public static void StartAudio(string key, EventReference audio, Vector3 position) {
            if (!ActiveAudio.ContainsKey(key) && !audio.IsNull) {
                EventInstance instance = RuntimeManager.CreateInstance(audio);

                ActiveAudio.Add(key, instance);
                instance.set3DAttributes(position.To3DAttributes());
                instance.start();
                instance.release();
            }
        }

        public static void StopAudio(string key) {
            if (ActiveAudio.ContainsKey(key)) {
                ActiveAudio[key].stop(STOP_MODE.ALLOWFADEOUT);
                ActiveAudio.Remove(key);
            }
        }

        public static void PlayGreatSuccess() {
            if (!Instance.jingleEvent.IsNull) {
                PlayOneShot(new OneShotConfig() {
                    eventReference = Instance.jingleEvent,
                    parameters = new [] {
                        new FmodParameter() { parameterName = "Jingle", value = "Great Success" },
                    }
                });
            }
        }
        
        public static void PlaySuccess() {
            if (!Instance.jingleEvent.IsNull) {
                PlayOneShot(new OneShotConfig() {
                    eventReference = Instance.jingleEvent,
                    parameters = new [] {
                        new FmodParameter() { parameterName = "Jingle", value = "Success" },
                    }
                });
            }
        }
        
        public static void PlayFailure() {
            if (!Instance.jingleEvent.IsNull) {
                PlayOneShot(new OneShotConfig() {
                    eventReference = Instance.jingleEvent,
                    parameters = new [] {
                        new FmodParameter() { parameterName = "Jingle", value = "Failure" },
                    }
                });
            }
        }
        
        public static void PlayOneShot(EventReference audio) {
            RuntimeManager.PlayOneShot(audio);
        }
        
        public static void PlayOneShot(OneShotConfig config) {
            if (config.eventReference.IsNull) return;
            
            EventInstance instance = RuntimeManager.CreateInstance(config.eventReference);

            if (config.attachedGameObject) {
                RuntimeManager.AttachInstanceToGameObject(instance, config.attachedGameObject);
            }
            else {
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(config.position));
            }

            if (config.parameters != null) {
                foreach (FmodParameter parameter in config.parameters) {
                    parameter.AddParameterToInstance(instance);
                }
            }
            
            instance.start();
            instance.release();
        }

        private void Awake() {
            Instance = this;
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
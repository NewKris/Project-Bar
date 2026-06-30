using System.Collections.Generic;
using Runtime.Configuration;
using UnityEngine;

namespace Runtime.Drink {
    public class Shaker : DrinkObject {
        
        public void TickShake() {
            ShakeDuration += Time.deltaTime;
            if (ShakeDuration >= Config.Instance.shakeDurationInSeconds) {
                currentContents.mixType = MixType.Shaken;
            }
        }

        public void CreateMixTimer(int key) {
            MixDurations.Add(key, 0);
        }

        public bool HasMixTimer(int key) {
            return  MixDurations.ContainsKey(key);
        }

        public void TickMix(int key) {
            if (MixDurations.ContainsKey(key)) {
                MixDurations[key] += Time.deltaTime;
            }
        }

        public void RemoveMixerKey(int key) {
            MixDurations.Remove(key);
        }

        public float GetMixAmount(int key) {
            return  MixDurations.GetValueOrDefault(key, 0);
        }
    }
}
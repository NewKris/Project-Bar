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
    }
}
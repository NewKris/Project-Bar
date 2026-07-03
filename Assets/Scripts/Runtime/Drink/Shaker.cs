using System.Collections.Generic;
using Runtime.Configuration;
using UnityEngine;

namespace Runtime.Drink {
    public class Shaker : DrinkObject {
        public float shakeDuration = 0.5f;
        
        public void TickShake() {
            ShakeDuration += Time.deltaTime;
            if (ShakeDuration >= shakeDuration) {
                currentContents.mixType = MixType.Shaken;
            }
        }
    }
}
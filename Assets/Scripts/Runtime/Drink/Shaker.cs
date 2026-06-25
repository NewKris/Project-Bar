using Runtime.Configuration;
using UnityEngine;

namespace Runtime.Drink {
    public class Shaker : DrinkObject {
        
        public void TickShake() {
            shakeDuration += Time.deltaTime;
            if (shakeDuration >= Config.Instance.shakeDurationInSeconds) {
                currentContents.mixType = MixType.Shaken;
            }
        }
    }
}
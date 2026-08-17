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
            
            TryDestroyDrink();
        }

        private void TryDestroyDrink() {
            if (currentContents.isDestroyed || !currentContents.ContainsPrepOrGarnish()) return;

            currentContents.isDestroyed = true;
        }
    }
}
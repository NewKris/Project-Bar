using System;
using UnityEngine;

namespace Runtime.Old_Systems.Drink {
    [Obsolete]
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
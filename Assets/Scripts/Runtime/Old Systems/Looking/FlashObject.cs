using System.Collections;
using Runtime.Drinks;
using UnityEngine;

namespace Runtime.Old_Systems.Looking {
    [RequireComponent(typeof(LookMaterial))]
    public class FlashObject : MonoBehaviour {
        public Ingredient[] ingredients;

        public void Flash(float duration) {
            StartCoroutine(FlashAsync(duration));
        }

        private IEnumerator FlashAsync(float duration) {
            GetComponent<LookMaterial>().StartFlash();
            yield return new WaitForSeconds(duration);
            GetComponent<LookMaterial>().EndFlash();
        }
    }
}
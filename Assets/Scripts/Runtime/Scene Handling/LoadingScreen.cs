using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Scene_Handling {
    public class LoadingScreen : MonoBehaviour {
        public float fadeDuration;
        public float padding = 0.1f;
        public Image fadeImage;

        public IEnumerator FadeIn() {
            yield return new WaitForSeconds(padding);
            yield return Fade(Color.black, Color.clear, fadeDuration);
        }

        public IEnumerator FadeOut() {
            yield return Fade(Color.clear, Color.black, fadeDuration);
            yield return new WaitForSeconds(padding);
        }

        private IEnumerator Fade(Color from, Color to, float duration) {
            for (float t = 0; t < duration; t += Time.deltaTime) {
                fadeImage.color = Color.Lerp(from, to, t / duration);
                
                yield return null;
            }
            
            fadeImage.color = to;
        }
    }
}
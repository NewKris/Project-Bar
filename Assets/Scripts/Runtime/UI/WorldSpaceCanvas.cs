using Runtime.Looking;
using UnityEngine;

namespace Runtime.UI {
    public class WorldSpaceCanvas : MonoBehaviour {
        private static WorldSpaceCanvas Instance;
        
        public GameObject promptTextPrefab;
        public GameObject fillCirclePrefab;

        public PromptText CreatePromptText(string text, Vector3 position, Quaternion rotation) {
            PromptText promptText = InstantiatePrefab<PromptText>(promptTextPrefab, position, rotation);
            promptText.Initialize(text, position, rotation);

            return promptText;
        }

        public FillCircle CreateFillCircle(float startAmount, Vector3 position, Quaternion rotation) {
            FillCircle fillCircle = InstantiatePrefab<FillCircle>(fillCirclePrefab, position, rotation);
            fillCircle.Fill = startAmount;
            
            return fillCircle;
        }

        private T InstantiatePrefab<T>(GameObject prefab, Vector3 position, Quaternion rotation) where T : Component {
            return Instantiate(prefab, position, rotation, transform).GetComponent<T>();
        }
    }
}
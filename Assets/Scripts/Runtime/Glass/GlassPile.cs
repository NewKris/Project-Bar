using NaughtyAttributes;
using UnityEngine;

namespace Runtime.Glass {
    public class GlassPile : MonoBehaviour {
        [ShowAssetPreview(128, 128)]
        public GameObject glassPrefab;
        
        public void GrabGlass() {
            DrinkGlass newGlass = Instantiate(glassPrefab).GetComponent<DrinkGlass>();
            newGlass.PickUp();
        }
    }
}
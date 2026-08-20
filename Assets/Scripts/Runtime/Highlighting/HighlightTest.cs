using Runtime.Interaction;
using UnityEngine;

namespace Runtime.Highlighting {
    public class HighlightTest : MonoBehaviour, IHoverInteraction {
        public void BeginHover() {
            Debug.Log("Begin hover");
        }

        public void EndHover() {
            Debug.Log("End hover");
        }
    }
}
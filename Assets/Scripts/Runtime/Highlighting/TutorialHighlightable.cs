using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Highlighting {
    public class TutorialHighlightable : MonoBehaviour {
        private bool _highlighted = false;
        public UnityAction onClicked;
        
        public void Highlight() {
            _highlighted = true;
        }

        public void Click() {
            if (!_highlighted) return;
            onClicked?.Invoke();
            _highlighted = false;
        }
        
    }
}
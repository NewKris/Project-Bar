using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Highlighting {
    public class Highlightable : MonoBehaviour {
        private bool _tutorialClickable;
        private bool _lookedAt;
        public UnityAction onClicked;
        
        [SerializeField] private GameObject meshGameObject;

        [SerializeField] [Layer] private int defaultLayer;
        [SerializeField] [Layer] private int tutorialHighlightLayer;
        [SerializeField] [Layer] private int lookHighlightLayer;
        
        private void UpdateState(bool tutorialClickable, bool lookedAt) {
            _tutorialClickable = tutorialClickable;
            _lookedAt = lookedAt;

            if (_lookedAt) {
                meshGameObject.layer = lookHighlightLayer;
            } else if (_tutorialClickable) {
                meshGameObject.layer = tutorialHighlightLayer;
            }
            else {
                meshGameObject.layer = defaultLayer;
            }
        }

        public void LookAtHighlight(bool lookedAt) {
            UpdateState(_tutorialClickable, lookedAt);
        }
        
        public void TutorialHighlight() {
            UpdateState(true, _lookedAt);
        }

        public void Click() {
            if (!_tutorialClickable) return;
            onClicked?.Invoke();
            UpdateState(false, _lookedAt);
        }
        
    }
}
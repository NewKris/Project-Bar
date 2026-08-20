using System;
using NaughtyAttributes;
using UnityEngine;

namespace Runtime.UI {
    public class PauseMenu : MonoBehaviour {
        [Required] public UIMethods methods;

        public void Toggle() {
            gameObject.SetActive(!gameObject.activeSelf);
        }
        
        private void OnEnable() {
            methods.SetPauseState(true);
        }

        private void OnDisable() {
            methods.SetPauseState(false);
        }
    }
}

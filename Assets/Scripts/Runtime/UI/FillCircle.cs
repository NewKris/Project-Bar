using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI {
    public class FillCircle : MonoBehaviour {
        private Image _image;
        
        public float Fill {
            get => _image.fillAmount;
            set => _image.fillAmount = value;
        }

        private void Awake() {
            _image = GetComponent<Image>();
        }
    }
}
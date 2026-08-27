using System;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Settings {
    [RequireComponent(typeof(Slider))]
    public abstract class SettingsSlider : MonoBehaviour {
        protected abstract string KeyName { get; }

        protected abstract void ApplySetting(float value);
        
        private void Start() {
            Slider slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(SaveValue);
            slider.value = PlayerPrefs.GetFloat(KeyName, slider.maxValue);
        }

        private void SaveValue(float value) {
            ApplySetting(value);
            PlayerPrefs.SetFloat(KeyName, value);
        }
    }
}
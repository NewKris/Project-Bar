using System;
using UnityEngine;

namespace Runtime.Settings {
    public class SettingBlackBoard : MonoBehaviour {
        public const string CAMERA_SENSITIVITY_KEY = "CameraSensitivity";
        
        public static float CameraSensitivity { get; set; }

        private void Awake() {
            LoadPrefs();
        }

        private void LoadPrefs() {
            CameraSensitivity = PlayerPrefs.GetFloat(CAMERA_SENSITIVITY_KEY, 1);
        }
    }
}
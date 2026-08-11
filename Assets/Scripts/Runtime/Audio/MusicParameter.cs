using System;
using UnityEngine;

namespace Runtime.Audio {
    public class MusicParameter : MonoBehaviour {
        public string parameterName;
        public string parameterValue;
        
        private void Start() {
            MusicManager.SetParameter(parameterName, parameterValue);
        }
    }
}
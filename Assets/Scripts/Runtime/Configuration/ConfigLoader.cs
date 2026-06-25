using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Runtime.Configuration {
    public class ConfigLoader : MonoBehaviour {
        private void Awake() {
            StartCoroutine(LoadConfigAsync());
        }

        private IEnumerator LoadConfigAsync() {
            Task task = Config.Load();
            while (!task.IsCompleted) {
                yield return null;
            }
        }
    }
}
using System;
using System.Collections;
using System.Threading.Tasks;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Configuration {
    public class ConfigLoader : MonoBehaviour {
        public static event Action<Config> OnConfigLoaded;
        
        private static ConfigLoader Instance;
        
        private void Awake() {
            if (Singleton.SetSingleton(ref Instance, this)) {
                StartCoroutine(LoadConfigAsync());
            }
        }

        private void OnDestroy() {
            Singleton.UnsetSingleton(ref Instance, this);
        }

        private IEnumerator LoadConfigAsync() {
            Task task = Config.Load();
            while (!task.IsCompleted) {
                yield return null;
            }
            
            OnConfigLoaded?.Invoke(Config.instance);
        }
    }
}
using System;
using System.Threading.Tasks;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Configuration {
    [Serializable]
    public class Config {
        public static Config Instance = new Config();
        
        private const string FilePath = "/Config/config";
        
        public float shakeDurationInSeconds = 1f;

        public static async Task Load() {
            if (FileManager.FileExists(FilePath)) {
                Instance = await FileManager.DeserializeFile<Config>(FilePath);
            }
            else {
                Instance = new Config();
                await FileManager.SerializeFile(Instance, FilePath);
            }
            
        }
    }
}
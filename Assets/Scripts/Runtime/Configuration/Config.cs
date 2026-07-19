using System;
using System.Threading.Tasks;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Configuration {
    [Serializable]
    public class Config {
        public static Config instance = new Config();
        
        private const string FILE_PATH = "/Config/config";

        public float patienceMultiplier = 1;
        public bool verboseLogging = false;
        
        public static async Task Load() {
            if (FileManager.FileExists(FILE_PATH)) {
                instance = await FileManager.DeserializeFile<Config>(FILE_PATH);
                
            }
            else {
                instance = new Config();
            }

            await FileManager.SerializeFile(instance, FILE_PATH);
        }
    }
}
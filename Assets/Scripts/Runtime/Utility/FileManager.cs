using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Runtime.Utility {
    internal static class FileManager {
        private const string FILE_TYPE = ".json";
        private const string META_EXTENSION = ".meta";
        
        private static string GamePath {
            get {
#if UNITY_EDITOR
                return Application.dataPath;
#else
                return Application.persistentDataPath;
#endif
            }
        }

        public static void CreateDirectory(string directory) {
            string absolutePath = CreateAbsolutePath(directory);
            
            Directory.CreateDirectory(absolutePath);
            Debug.Log("Created directory: " + absolutePath);
            
            UpdateFileSystem();
        }

        public static bool FileExists(string localPath) {
            return File.Exists(CreateAbsolutePath(localPath) + FILE_TYPE);
        }
        
        public static async Task SerializeFile<T>(T data, string localPath) {
            string absolutePath = CreateAbsolutePath(localPath) + FILE_TYPE;
            Debug.Log("Serializing: " + absolutePath);
            
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath) ?? "");
            string json = JsonUtility.ToJson(data, true);
            await File.WriteAllTextAsync(absolutePath, json);
            UpdateFileSystem();
            
            Debug.Log("File saved: " + absolutePath);
        }

        public static async Task<T> DeserializeFile<T>(string localPath) {
            string absolutePath = CreateAbsolutePath(localPath) + FILE_TYPE;
            Debug.Log("Deserializing: " + absolutePath);

            if (!File.Exists(absolutePath)) {
                return JsonUtility.FromJson<T>("");
            }
            
            string json = await File.ReadAllTextAsync(absolutePath);
            Debug.Log("File Loaded: " + absolutePath);
            
            return JsonUtility.FromJson<T>(json);
        }

        public static void DeleteFile(string localPath) {
            string absolutePath = CreateAbsolutePath(localPath) + FILE_TYPE;
            Debug.Log("Deleting: " + absolutePath);
            
            if (File.Exists(absolutePath)) {
                File.Delete(absolutePath);
            }
            
            if (File.Exists(absolutePath + META_EXTENSION)) {
                File.Delete(absolutePath + META_EXTENSION);
            }
            
            UpdateFileSystem();
        }

        private static string CreateAbsolutePath(string localPath) {
            return GamePath + localPath;
        }

        private static void UpdateFileSystem() {
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}

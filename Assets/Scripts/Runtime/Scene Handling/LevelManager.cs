using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Scene_Handling {
    public class LevelManager : MonoBehaviour, IGameSceneStartupTask {
        public static int currentLevelIndex = 0;
        public static int levelCount = 0;
        
        public GameScene[] levelScenes;
        
        public static bool ReachedTheEnd => currentLevelIndex >= levelCount;
        
        public IEnumerator StartupTask() {
            yield return SceneManager.LoadSceneAsync(levelScenes[currentLevelIndex].sceneName, LoadSceneMode.Additive);
        }
        
        private void Awake() {
            SceneTransitionController.startupTasks.Add(this);
            levelCount  = levelScenes.Length;
        }
    }
}
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Scene_Handling {
    public class LevelManager : MonoBehaviour, IGameSceneStartupTask {
        public int currentLevelIndex;
        public GameScene[] levelScenes;

        public IEnumerator StartupTask() {
            yield return SceneManager.LoadSceneAsync(levelScenes[currentLevelIndex].sceneName, LoadSceneMode.Additive);
        }
        
        private void Awake() {
            SceneTransitionController.startupTasks.Add(this);
        }
    }
}
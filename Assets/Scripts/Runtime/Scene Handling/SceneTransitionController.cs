using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Scene_Handling {
    public class SceneTransitionController : MonoBehaviour {
        public static readonly HashSet<IGameSceneStartupTask> startupTasks = new HashSet<IGameSceneStartupTask>();
        
        private static SceneTransitionController Instance;
        private static bool Busy;
        
        public LoadingScreen loadingScreen;
        
        public static void TransitionToScene(GameScene gameScene) {
            if (Busy || !Instance) return;

            Instance.StartCoroutine(Instance.TransitionToSceneAsync(gameScene));
        }

        private void Awake() {
            if (Singleton.SetSingleton(ref Instance, this)) {
                Busy = false;
            }
        }

        private void OnDestroy() {
            Singleton.UnsetSingleton(ref Instance, this);
        }

        private IEnumerator TransitionToSceneAsync(GameScene gameScene) {
            Busy = true;
            yield return loadingScreen.FadeOut();
            
            yield return SceneManager.LoadSceneAsync(gameScene.sceneName, LoadSceneMode.Single);
            
            foreach (IGameSceneStartupTask gameSceneStartupTask in startupTasks) {
                yield return gameSceneStartupTask.StartupTask();
            }

            startupTasks.Clear();
            
            yield return loadingScreen.FadeIn();
            Busy = false;
        }
    }
}
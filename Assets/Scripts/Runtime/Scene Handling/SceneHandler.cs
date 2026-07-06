using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Scene_Handling
{
    [CreateAssetMenu(fileName = "Scene Handler", menuName = "Scene Handling/Scene Handler", order = 0)]
    public class SceneHandler : ScriptableObject
    {
        public GameScene gameplayScene;
        public GameScene mainMenuScene;
        public GameScene gameOverScreen;
        public GameScene victoryScreen;

        // public async void LoadLevelWithIndex(int index) {
        //     if (index < 0 || index >= levels.Count) {
        //         Debug.LogWarning($"{name}-Scene Manager: Scene Index out of range.");
        //         currentLevelIndex = 0;
        //         return;
        //     }
        //     
        //     await SceneManager.LoadSceneAsync(levels[index].sceneName, LoadSceneMode.Additive);
        // }
        
        public void StartGame() {
            SceneTransitionController.TransitionToScene(gameplayScene);
        }

        public void GameOver() {
            SceneTransitionController.TransitionToScene(gameOverScreen);
        }
        
        public void Victory() {
            SceneTransitionController.TransitionToScene(victoryScreen);
        }
        
        public void MainMenu() {
            SceneTransitionController.TransitionToScene(mainMenuScene);
        }
    }
}

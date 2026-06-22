using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Scene_Handling
{
    [CreateAssetMenu(fileName = "Scene Handler", menuName = "Scene Handling/Scene Handler", order = 0)]
    public class SceneHandler : ScriptableObject
    {
        public GameScene GameplayScene;
        public GameScene CoreScene;
        public GameScene MainMenuScene;
        public List<GameScene> Levels = new List<GameScene>();
        public int CurrentLevelIndex = 0;

        public void LoadLevelWithIndex(int index)
        {
            if (index < 0 || index >= Levels.Count)
            {
                Debug.LogWarning($"{name}-Scene Manager: Scene Index out of range.");
                CurrentLevelIndex = 0;
                return;
            }
            SceneManager.LoadSceneAsync(Levels[index].Name, LoadSceneMode.Additive);
        }
        
        public void StartGame()
        {
            CurrentLevelIndex = 0;
            SceneManager.LoadSceneAsync(CoreScene.Name, LoadSceneMode.Single);
            SceneManager.LoadSceneAsync(GameplayScene.Name, LoadSceneMode.Additive);
            LoadLevelWithIndex(0);
        }

        public void NextLevel()
        {
            SceneManager.UnloadSceneAsync(Levels[CurrentLevelIndex].Name);
            LoadLevelWithIndex(CurrentLevelIndex + 1);
        }

        public void MainMenu()
        {
            SceneManager.LoadSceneAsync(MainMenuScene.Name, LoadSceneMode.Single);
        }
    }
}
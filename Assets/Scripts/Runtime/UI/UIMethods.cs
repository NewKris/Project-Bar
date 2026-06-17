using UnityEngine;
using Runtime.Scene_Handling;

namespace Runtime.UI
{
    [CreateAssetMenu(fileName = "UI Methods", menuName = "Scriptable Objects/UI Methods", order = 0)]
    public class UIMethods : ScriptableObject
    {
        [SerializeField] private SceneHandler sceneHandler;
        private GameModifiers _gameModifiers;

        public void QuitGame()
        {
            Application.Quit();
        }

        public void QuitToMainMenu()
        {
            sceneHandler.MainMenu();
        }

        public void TogglePause()
        {
            if (Time.timeScale > 1f)
            {
                Time.timeScale = 0f;
            }
            else
            {
                _gameModifiers ??= GameModifiers.GetInstance();
                Time.timeScale = _gameModifiers.ActiveTimeScale;
            }
        }

        public void SetPauseState(bool pause)
        {
            if (pause) Time.timeScale = 0f;
            else
            {
                _gameModifiers ??= GameModifiers.GetInstance();
                Time.timeScale = _gameModifiers.ActiveTimeScale;
            }
        }
    }
}
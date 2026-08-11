using System.Collections;
using Runtime.Configuration;
using Runtime.Customers;
using Runtime.Satisfaction;
using Runtime.Scene_Handling;
using Runtime.Utility;
using UnityEngine;

namespace Runtime.Gameplay {
    public enum GameOverReason {
        None,
        Satisfaction,
        WrongTarget,
    }
    
    public class GameplayManager : MonoBehaviour {
        private static bool GameEnded;

        public GameplayPort gameplayPort;
        public SceneHandler sceneHandler;
        public SatisfactionEvents satisfactionEvents;
        public CustomerEvents customerEventPort;
        public float gameEndWait = 5;

        private void Awake() {
            satisfactionEvents.OnGameOver += SatisfactionGameOver;
            customerEventPort.OnCustomerDied += EvaluateAssassination;
            ConfigLoader.OnConfigLoaded += SetLoggingLevel;

            GameEnded = false;
            
            SetLoggingLevel(Config.instance);
        }

        private void OnDestroy() {
            satisfactionEvents.OnGameOver -= SatisfactionGameOver;
            customerEventPort.OnCustomerDied -= EvaluateAssassination;
            ConfigLoader.OnConfigLoaded -= SetLoggingLevel;
        }

        private void SatisfactionGameOver() {
            if (GameEnded) return;
            
            GameOverSubtitle.reason = GameOverReason.Satisfaction;
            StartCoroutine(EndGame(false));
        }

        private void WrongTargetGameOver() {
            if (GameEnded) return;
            
            GameOverSubtitle.reason = GameOverReason.WrongTarget;
            StartCoroutine(EndGame(false));
        }

        private void EvaluateAssassination(bool targetKilled) {
            if (targetKilled) {
                StartCoroutine(EndGame(true));
            }
            else {
                WrongTargetGameOver();
            }
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(Screen.width - 110, 10, 100, 300));
            
            if (GUILayout.Button("Exit Game")) {
                sceneHandler.MainMenu();
            }
            
            GUILayout.EndArea();
        }

        private void SetLoggingLevel(Config config) {
            VerboseDebug.enableVerboseLogging = config.verboseLogging;
        }

        private IEnumerator EndGame(bool victory) {
            GameEnded = true;
            gameplayPort.EndGameplay();
            
            yield return new WaitForSeconds(gameEndWait);
            
            if (victory) sceneHandler.Victory();
            else sceneHandler.GameOver();
        }
    }
}
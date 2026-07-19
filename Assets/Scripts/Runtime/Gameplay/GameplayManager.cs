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
        public SceneHandler sceneHandler;
        public SatisfactionEvents satisfactionEvents;
        public CustomerEvents customerEventPort;

        private void Awake() {
            satisfactionEvents.OnGameOver += SatisfactionGameOver;
            customerEventPort.OnCustomerDied += EvaluateAssassination;
            ConfigLoader.OnConfigLoaded += SetLoggingLevel;
            
            SetLoggingLevel(Config.instance);
        }

        private void OnDestroy() {
            satisfactionEvents.OnGameOver -= SatisfactionGameOver;
            customerEventPort.OnCustomerDied -= EvaluateAssassination;
            ConfigLoader.OnConfigLoaded -= SetLoggingLevel;
        }

        private void SatisfactionGameOver() {
            GameOverSubtitle.reason = GameOverReason.Satisfaction;
            sceneHandler.GameOver();
        }

        private void WrongTargetGameOver() {
            GameOverSubtitle.reason = GameOverReason.WrongTarget;
            sceneHandler.GameOver();
        }

        private void EvaluateAssassination(bool targetKilled) {
            if (targetKilled) {
                sceneHandler.Victory();
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
    }
}
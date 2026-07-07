using System;
using Runtime.Customers;
using Runtime.Satisfaction;
using Runtime.Scene_Handling;
using UnityEngine;

namespace Runtime {
    public class GameplayManager : MonoBehaviour {
        public SceneHandler sceneHandler;
        public SatisfactionEvents satisfactionEvents;
        public CustomerEvents customerEventPort;

        private void Awake() {
            satisfactionEvents.OnGameOver += TriggerGameOver;
            customerEventPort.OnCustomerDied += EvaluateAssassination;
        }

        private void OnDestroy() {
            satisfactionEvents.OnGameOver -= TriggerGameOver;
            customerEventPort.OnCustomerDied -= EvaluateAssassination;
        }

        private void TriggerGameOver() {
            sceneHandler.GameOver();
        }

        private void EvaluateAssassination(bool targetKilled) {
            if (targetKilled) {
                sceneHandler.Victory();
            }
            else {
                TriggerGameOver();
            }
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(Screen.width - 110, 10, 100, 300));
            
            if (GUILayout.Button("Exit Game")) {
                sceneHandler.MainMenu();
            }
            
            GUILayout.EndArea();
        }
    }
}
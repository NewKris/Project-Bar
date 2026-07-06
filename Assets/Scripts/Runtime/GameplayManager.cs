using System;
using Runtime.Satisfaction;
using Runtime.Scene_Handling;
using UnityEngine;

namespace Runtime {
    public class GameplayManager : MonoBehaviour {
        public SceneHandler sceneHandler;
        public SatisfactionEvents satisfactionEvents;

        private void Awake() {
            satisfactionEvents.OnGameOver += TriggerGameOver;
        }

        private void OnDestroy() {
            satisfactionEvents.OnGameOver -= TriggerGameOver;
        }

        private void TriggerGameOver() {
            sceneHandler.GameOver();
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
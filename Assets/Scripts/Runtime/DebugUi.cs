using System;
using System.Linq;
using Runtime.Drink;
using Runtime.Player;
using Runtime.Satisfaction;
using Runtime.Utility;
using UnityEngine;

namespace Runtime {
    public class DebugUi : MonoBehaviour {
        private SatisfactionManager _satisfactionManager;
        private PlayerHand _playerHand;

        private void Awake() {
            _playerHand = FindObjectsByType<PlayerHand>(FindObjectsSortMode.None).FirstOrDefault();
            _satisfactionManager = FindObjectsByType<SatisfactionManager>(FindObjectsSortMode.None).FirstOrDefault();
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 500, 500));

            if (_satisfactionManager) {
                GUILayout.Label($"Satisfaction: {_satisfactionManager.currentSatisfaction}");
            }
            
            if (_playerHand?.HeldItem?.TryGetComponent(out DrinkObject drink) ?? false) {
                GUILayout.Label($"Container: {drink.currentContents.drinkContainer?.name ?? "NONE"}");
                GUILayout.Label($"Mix: {drink.currentContents.mixType}");
                
                foreach (Ingredient ingredient in drink.currentContents.ingredients) {
                    GUILayout.Label(ingredient.name);
                }
                
            }
            
            GUILayout.EndArea();
        }
    }
}
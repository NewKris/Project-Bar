using Runtime.Customers.Spawning;
using Runtime.Drinks;
using Runtime.Old_Systems.Drink;
using Runtime.Old_Systems.Player.Hand;
using Runtime.Player;
using Runtime.Satisfaction;
using UnityEngine;

namespace Runtime.Gameplay {
    public class DebugUi : MonoBehaviour {
        private SatisfactionManager _satisfactionManager;
        private PlayerHand _playerHand;
        private CustomerManager _customerManager;

        private void Awake() {
            _playerHand = FindAnyObjectByType<PlayerHand>();
            _satisfactionManager = FindAnyObjectByType<SatisfactionManager>();
            _customerManager = FindAnyObjectByType<CustomerManager>();
        }

        private void OnGUI() {
            GUIStyle textStyle = CreateBoxStyle();
            GUILayout.BeginArea(new Rect(10, 10, 200, 1000));

            if (_satisfactionManager) {
                GUILayout.Label($"Satisfaction: {_satisfactionManager.currentSatisfaction} / {_satisfactionManager.TargetSatisfaction}", textStyle);
                GUILayout.Label($"Target Can Spawn: {_satisfactionManager.TargetCanSpawn}", textStyle);
            }

            if (_customerManager) {
                GUILayout.Label($"Target Spawn Chance: {_customerManager.TargetSpawnChance}%", textStyle);
            }
            
            if (_playerHand?.HeldItem?.TryGetComponent(out DrinkObject drink) ?? false) {
                GUILayout.Label($"Container: {drink.currentContents.drinkContainer?.name ?? "NONE"}", textStyle);

                textStyle.fontSize -= 4;
                foreach (IngredientGroup group in drink.currentContents.ingredientGroups) {
                    foreach (Ingredient ingredient in group.ingredients) {
                        GUILayout.Label(ingredient.name, textStyle);
                    }

                    GUILayout.Space(10);
                }
                
            }
            
            GUILayout.EndArea();
        }

        private GUIStyle CreateBoxStyle() {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
            tex.Apply();
            
            GUIStyle boxStyle = new GUIStyle(GUI.skin.box) {
                alignment = TextAnchor.UpperLeft,
                fontSize = 14,
                normal = {
                    textColor = Color.white,
                    background = tex
                },
            };

            return boxStyle;
        }
    }
}